using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ������
/// </summary>
public class StageGenerator : MonoBehaviour
{
    const int StageClipSize = 30;

    int currentClipIndex;
    // �^�[�Q�b�g�L�����N�^�[�̎w��
    public Transform character;
    // �X�e�[�W�`�b�v�v���n�u�z��
    public GameObject[] stageChips;
    // ���������J�n�C���f�b�N�X
    public int startChipIndex;
    // ������ǂ݌�
    public int preInstantiate;

    public List<GameObject> generatedStageList = new List<GameObject>();

    /// <summary>
    /// ����������
    /// </summary>
    void Start()
    {
        currentClipIndex = startChipIndex - 1;
        UpdateStage(preInstantiate);
        
    }

    void Update()
    {
        // �L�����N�^�[�̈ʒu���猻�݂̃X�e�[�W�`�b�v�̃C���f�b�N�X�̌v�Z
        int charaPositionIndex = (int)(character.position.z / StageClipSize);

        // ���̃X�e�[�W�`�b�v�ɓ�������X�e�[�W�̍X�V�������s��
        if(charaPositionIndex + preInstantiate > currentClipIndex)
        {
            UpdateStage(charaPositionIndex + preInstantiate);
        }
    }
    
    /// <summary>
    /// �w���Index�܂ŃX�e�[�W�`�b�v�𐶐����ĊǗ�����
    /// </summary>
    /// <param name="toChipIndex">�w��̃C���f�b�N�X</param>
    void UpdateStage(int toChipIndex)
    {
        if(toChipIndex <= currentClipIndex)
        {
            return;
        }

        // �w��̃X�e�[�W�`�b�v�܂ō쐬
        for(int i = currentClipIndex+1;i<=toChipIndex;i++)
        {
            GameObject stageObject = GenerateStage(i);

            //���������X�e�[�W�`�b�v���Ǘ����X�g�ɒǉ�
            generatedStageList.Add(stageObject);
        }

        // �X�e�[�W�̕ێ�������ɂȂ�܂ŌÂ��X�e�[�W���폜 
        while (generatedStageList.Count > preInstantiate + 2)
        {
            DestroyOldestStage();
        }
        currentClipIndex = toChipIndex;

    }

    /// <summary>
    /// �w��̃C���f�b�N�X�ʒu��StageObject�������_���ɐ���
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
    /// ��ԌÂ��X�e�[�W�I�u�W�F�N�g�̍폜
    /// </summary>
    void DestroyOldestStage()
    {
        GameObject oldStage = generatedStageList[0];
        generatedStageList.RemoveAt(0);
        Destroy(oldStage);
    }


}
