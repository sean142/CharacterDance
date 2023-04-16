using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class ChangeModel : MonoBehaviour
{
    public static ChangeModel instance;
    //模型陣列
    public GameObject[] models;
    //當前模型的狀態
    public GameObject[] currentModelsState;   
    //選中的顏色
    private Color selectedColor;
    //顏色陣列
    public Color[] colors;
    //所選中的物體位置
    public Transform selectedTransform;
    //旋轉量
    public float rotateSpeed;
    //下拉式選單
    public Dropdown dropdown;
    //音源
    public AudioSource audioSource;
    //燈光陣列
    public Light[] lights;
    //燈關切換間隔
    public float timeInterval = 1.0f;
    //當前燈光索引
    public int currentLightIndex = 0;
    //燈光位置偏移量
    public Vector3 offset;
    [SerializeField]
    private Animator animator;
    //燈是否打開
    public bool isLightOn;
    //判斷按下六軸按鈕bool
    public bool[] move;
    //計時器
    public bool timer;
    //要移動模型的速度
    public float speed;
    //是否在跳舞
    public bool isDance;
    //紀錄上個模型編號
    public int lastIndex;
    //選中的模型的索引
    public int selectedModelIndex = 0;

    private void Awake()
    {
        instance = this;
    }

    void Update()
    {
        for (int i = 0; i < 8; i++)//用簡單的迴圈判斷當前按下的六軸按鈕
        {
            if (move[i])//i數值對應按鈕號碼
            {
                if (timer)
                {
                    StartCoroutine(Move());//執行六軸用Coroutine(做這個是方便按住按鈕時也能持續有效)
                    timer = false;//計時器啟動2(下面有1)
                }
            }
        }
    }

    void SwitchLight()
    {
        // 關閉目前燈光
        lights[currentLightIndex].gameObject.SetActive(false);

        // 切換到下一個燈光
        currentLightIndex++;
        if (currentLightIndex >= lights.Length)
        {
            currentLightIndex = 0;
        }

        // 打開下一個燈
        lights[currentLightIndex].gameObject.SetActive(true);

        // 移動燈光到當前角色位置
        lights[currentLightIndex].transform.position = transform.position+offset;
    }
    public void MoveUp()
    {
        StartCoroutine("Move");
        move[0] = true;

        // selectedTransform.Translate(0f, 1f, 0f);
    }
    public void MoveDown()
    {
        //if (selectedTransform.position.y <= 0)
        //{
        //    return;
        //}
        StartCoroutine("Move");
        move[1] = true;
        // selectedTransform.Translate(0f, -1f, 0f);
    }
    public void MoveLeft()
    {
        StartCoroutine("Move");
        move[2] = true;

        // selectedTransform.Translate(1f, 0f, 0f);
    }
    public void MoveRight()
    {
        StartCoroutine("Move");
        move[3] = true;

        // selectedTransform.Translate(-1f, 0f, 0f);
    }
    public void MoveFront()
    {
        StartCoroutine("Move");
        move[4] = true;

        // selectedTransform.Translate(0f, 0f, 1f);
    }
    public void MoveBack()
    {
        StartCoroutine("Move");
        move[5] = true;

        //selectedTransform.Translate(0f, 0f, -1f);
    }

    public void LeftRotate()
    {
        StartCoroutine("Move");
        move[6] = true;

        // selectedTransform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);
    }
    public void RightRotate()
    {
        StartCoroutine("Move");
        move[7] = true;

        // selectedTransform.Rotate(-Vector3.up * rotateSpeed * Time.deltaTime);
    }

    public void CloseMove()//六軸按鈕鬆開(停止移動)
    {
        for (int i = 0; i < 8; i++)
        {
            move[i] = false;//全部移動編號關閉
        }
    }

    IEnumerator Move()
    {
        yield return new WaitForFixedUpdate();
        if (move[0])
        {
            currentModelsState[1].transform.position += currentModelsState[1].transform.up * Time.deltaTime * speed;
        }
        else if (move[1])
        {
            if (currentModelsState[1].transform.position.y <= 0)
                yield break ;
            currentModelsState[1].transform.position += currentModelsState[1].transform.up * Time.deltaTime * -speed;
        }
        else if (move[2])
        {
            currentModelsState[1].transform.position += currentModelsState[1].transform.right * Time.deltaTime * speed;
        }
        else if (move[3])
        {
            currentModelsState[1].transform.position += currentModelsState[1].transform.right * Time.deltaTime * -speed;
        }
        else if (move[4])
        {
            currentModelsState[1].transform.position += currentModelsState[1].transform.forward * Time.deltaTime * speed;
        }
        else if (move[5])
        {
            currentModelsState[1].transform.position += currentModelsState[1].transform.forward * Time.deltaTime * -speed;
        }
        else if (move[6])
        {
            currentModelsState[1].transform.Rotate(new Vector3(0, speed, 0));
        }
        else if (move[7])
        {
            currentModelsState[1].transform.Rotate(new Vector3(0, -speed, 0));
        }
        timer = true;
    }
    public void ResetObject()
    {
        
        dropdown.value = 0;

        //回到初始狀態
        ApplyDefaultState();      
    }    
    
    public void OnMagicButton()
    {
        if (!isLightOn && !(selectedModelIndex==0) && !isDance)
        {
            animator.SetTrigger("Dance");

            //播放選擇模型的音效
            PlaySelectedModelAudio();

            //開始循環執行 SwitchLight 方法
            InvokeRepeating("SwitchLight", timeInterval, timeInterval);
            isLightOn = true;
            isDance = true;
        }       
    }
    public void PlaySelectedModelAudio()
    {
        //從所選模型的 transform 中取得 AudioSource 元件
        audioSource = selectedTransform.GetComponent<AudioSource>();

        // 播放該 AudioSource 元件的聲音
        audioSource.Play();
    }

    public void OnDropDownSelect(int index)
    {
      //  lastIndex = selectedModelIndex;
        // 記錄選擇的模型索引
        selectedModelIndex = index;    
    }      
           
    public void OnColorDropdownSelect(int index)
    {
        // 根據選擇的顏色索引設定所選顏色
        selectedColor = colors[index];

        // 套用所選顏色到當前模型狀態
        ApplyColorSelection();
    }
    // 選擇某個模型後，套用該模型的設定
    public void ApplySelection()
    {
        // 每次執行執行完更換模型，紀錄當前模型編號          
        if (selectedModelIndex != lastIndex)
        {
            for (int i = 0; i < models.Length; i++)
            {
                // 如果目前迴圈所在的索引值等於所選模型的索引值減一（因為索引值從零開始）
                if (i == selectedModelIndex - 1)
                {
                    isDance = false;

                    models[i].SetActive(true);
                    selectedTransform = models[i].transform;

                    // 取得該模型的 SkinnedMeshRenderer 和 GameObject 元件，並儲存到 currentModelsState 陣列中
                    currentModelsState[0] = models[i].transform.GetChild(0).gameObject;
                    currentModelsState[1] = models[i].transform.gameObject;

                    animator = models[i].GetComponent<Animator>();

                    if (isLightOn)
                    {
                        // 取消執行 SwitchLight 方法
                        CancelInvoke("SwitchLight");
                        // 關閉當前燈光
                        lights[currentLightIndex].gameObject.SetActive(false);
                        isLightOn = false;
                    }
                }
                else
                {
                    CancelInvoke("SwitchLight");
                    lights[currentLightIndex].gameObject.SetActive(false);
                    isLightOn = false;
                    models[i].SetActive(false);
                }
            }

            ApplyDefaultState();
            // 將下拉選單的索引值設為零
            dropdown.value = 0;
            lastIndex = selectedModelIndex;
        }
    }


    // 將該模型的位置、旋轉和材質顏色設為預設值
    private void ApplyDefaultState()
    {
        currentModelsState[1].transform.position = Vector3.zero;
        currentModelsState[1].transform.rotation = Quaternion.identity;
        currentModelsState[0].GetComponent<SkinnedMeshRenderer>().material.color = Color.white;
    }
    private void ApplyColorSelection()
    {
        // 將所選顏色套用到該模型的材質上
        currentModelsState[0].GetComponent<SkinnedMeshRenderer>().material.color = selectedColor;
    }       
}
