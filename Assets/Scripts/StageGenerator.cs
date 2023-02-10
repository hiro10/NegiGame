using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 床生成
/// </summary>
public class StageGenerator : MonoBehaviour
{
    const int StageClipSize = 30;

    int currentClipIndex;
    // ターゲットキャラクターの指定
    public Transform character;
    // ステージチッププレハブ配列
    public GameObject[] stageChips;
    // 自動生成開始インデックス
    public int startChipIndex;
    // 生成先読み個数
    public int preInstantiate;

    public List<GameObject> generatedStageList = new List<GameObject>();

    /// <summary>
    /// 初期化処理
    /// </summary>
    void Start()
    {
        currentClipIndex = startChipIndex - 1;
        UpdateStage(preInstantiate);
        
    }

    void Update()
    {
        // キャラクターの位置から現在のステージチップのインデックスの計算
        int charaPositionIndex = (int)(character.position.z / StageClipSize);

        // 次のステージチップに入ったらステージの更新処理を行う
        if(charaPositionIndex + preInstantiate > currentClipIndex)
        {
            UpdateStage(charaPositionIndex + preInstantiate);
        }
    }
    
    /// <summary>
    /// 指定のIndexまでステージチップを生成して管理する
    /// </summary>
    /// <param name="toChipIndex">指定のインデックス</param>
    void UpdateStage(int toChipIndex)
    {
        if(toChipIndex <= currentClipIndex)
        {
            return;
        }

        // 指定のステージチップまで作成
        for(int i = currentClipIndex+1;i<=toChipIndex;i++)
        {
            GameObject stageObject = GenerateStage(i);

            //生成したステージチップを管理リストに追加
            generatedStageList.Add(stageObject);
        }

        // ステージの保持上限内になるまで古いステージを削除 
        while (generatedStageList.Count > preInstantiate + 2)
        {
            DestroyOldestStage();
        }
        currentClipIndex = toChipIndex;

    }

    /// <summary>
    /// 指定のインデックス位置にStageObjectをランダムに生成
    /// </summary>
    /// <param name="chipIndex"></param>
    /// <returns></returns>
    GameObject GenerateStage(int chipIndex)
    {
        int nextStageClip = Random.Range(0, stageChips.Length);

        GameObject stageObject = (GameObject)Instantiate(
            stageChips[nextStageClip],
            new Vector3(0, 0, chipIndex * StageClipSize),
            Quaternion.identity);

        return stageObject;
    }

    /// <summary>
    /// 一番古いステージオブジェクトの削除
    /// </summary>
    void DestroyOldestStage()
    {
        GameObject oldStage = generatedStageList[0];
        generatedStageList.RemoveAt(0);
        Destroy(oldStage);
    }


}
