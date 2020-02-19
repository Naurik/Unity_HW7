using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.SceneManagement;

public class Monstr : MonoBehaviour
{
    [SerializeField]
    private Transform player;
    [SerializeField]
    private NavMeshAgent navMesh;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private Transform eyes;

    [SerializeField]
    private AudioSource scream;

    [Header("Camera Settings")]
    [SerializeField]
    private Transform deathCamera;
    [SerializeField]
    private Transform deathCameraPoint;

    private string state = "idle";
    private bool isAlive = true;
    private float waitTime = 2f;

    //режим повышенной встревожности
    private bool highAlertness = false;
    //уровень встревожности
    private float alertnessLevel = 20f;

    // Start is called before the first frame update
    void Start()
    {
        navMesh.speed=1f;
        animator.speed=1f;
        deathCamera.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(!isAlive){
            //делать что то 
            return;
        }
        //передаем в аниматор скорость персонажа
        animator.SetFloat("velocity",navMesh.velocity.magnitude);
           if(state =="idle"){
               GoToRandomPoint();
           }
           if(state == "walk"){
               StopWalking();
           }
           if(state == "search")
           {
               SearchForPlayer();
           }
           if(state == "chase")
           {
               ChaseForPlayer();
           }
           if(state == "hunt")
           {
               HuntForPlayer();
           }
           if(state == "kill"){
               
               //изменяем пощицию
               deathCamera.transform.position = Vector3.Slerp(deathCamera.position, deathCameraPoint.position, 10f*Time.deltaTime);
               deathCamera.transform.rotation = Quaternion.Slerp(deathCamera.rotation, deathCameraPoint.rotation, 10f*Time.deltaTime);
               animator.speed = 0.4f;
           }
    }

    //метод для проверки поля зрения
    public void CheckSight(){
        if(!isAlive){
            return;
        }
        RaycastHit hit;
        if(Physics.Linecast(eyes.position, player.position, out hit)){
            if(hit.collider.tag=="Player"){
                if(state == "kill"){
                    return;
                }
                scream.Play();
                //обновляем состояние
                state = "chase";
                navMesh.speed=2f;
                animator.speed =2f;
            }
        }
    }
    private void GoToRandomPoint(){
        //генерируем рандомную координату
        Vector3 randomPosition = Random.insideUnitSphere * 20f;
        //создаем переменную для хранения информации
        NavMeshHit navMeshHit;
        //ставим точку на mesh
        NavMesh.SamplePosition(transform.position + randomPosition, out navMeshHit, 20f, NavMesh.AllAreas);
        //если находится в режиме повышенной встревожности
        if(highAlertness){
            //ставим точку на mesh рядом с игроком
            NavMesh.SamplePosition(player.transform.position + randomPosition, out navMeshHit, 20f, NavMesh.AllAreas);
            //постепенно понижать уровень тревоги, если не нашел игрока
            alertnessLevel -=5f;
            //если уровень тревоги минимальный
            if(alertnessLevel <=0){
                //выйти из режима повышенной тревоги
                highAlertness = false;
                //сбросить 
                navMesh.speed = 1f;
                animator.speed = 1;
            }

        }


        //отправляем персонажа с сгенерированной точке
        navMesh.SetDestination(navMeshHit.position);
        //обновляем состояние
        state = "walk";

    }

    private void SearchForPlayer(){
        //если время ожидания больше 0
        if(waitTime > 0){
            //отнимаем время каждую секунду
            waitTime -= Time.deltaTime;
            //Вращаем персонажа на месте
            transform.Rotate(0,120f * Time.deltaTime, 0);
        }else{ //иначе
            //переходим в состояние idle
            state = "idle";
        }
    }
    private void StopWalking(){
        //Если оставшееся расстояние меньше чем конечное И не просчитывается новый маршрут
        if(navMesh.remainingDistance <= navMesh.stoppingDistance && !navMesh.pathPending){
            //обновляем
            state ="search";
           waitTime = 5f;
        }
    }
    private void ChaseForPlayer(){
        navMesh.SetDestination(player.position);
        //расстояние между персонажем и игроком
        float distance = Vector3.Distance(transform.position, player.transform.position);
        //если игрок оторвался
        if(distance > 10f){
            //искать на игрока
            state = "hunt";
        }else if(navMesh.remainingDistance <=navMesh.stoppingDistance && !navMesh.pathPending){
            Player playerController = player.GetComponent<Player>();
            if(playerController.isAlive){
                state = "kill";
                KillPlayer();
            }
        }
    }

    private void HuntForPlayer(){
        if(navMesh.remainingDistance <= navMesh.stoppingDistance && !navMesh.pathPending){
            //обновляем state
            state = "search";
            //указываем время ожидания
            waitTime =5f;
            //включаем уровень встревожности
            highAlertness = true;
            //устанавливаем уровень тревоги
            alertnessLevel = 20f;
            //
            CheckSight();
        }
    }

    private void KillPlayer(){
        //запускаем анимацию
        animator.SetTrigger("Kill");
        //обновляем значение переменной isAlive и игрока
        player.GetComponent<Player>().isAlive =false;
        //отключаем управление игрока
        player.GetComponent<RigidbodyFirstPersonController>().enabled=false;
        //включаем объект deathCamera
        deathCamera.gameObject.SetActive(true);
        //помещаем deathCamera в ту позицию где была камера игрока
        deathCamera.position = Camera.main.transform.position;
        deathCamera.rotation = Camera.main.transform.rotation;
        //отключаем камеру игрока
        Camera.main.gameObject.SetActive(false);
        //Воспроизвести звук
        //перезапустить игру
        Invoke("RestartGame", 2f);
    }

    private void RestartGame(){
        //перезапускаем уровень 
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 20f);    
    }
}
