using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SpawnPoint
{
    public string key; // Ex: "vindo_da_esquerda", "porta_secreta"
    public float xPosition; // Como é sidescroller, precisamos apenas do X
}

[System.Serializable]
public struct TransitionTrigger
{
    public float triggerX; // Posição X onde a transição fica ativa
    public float range; // Margem de erro (ex: se o player estiver a 0.5f de distância)
    public string targetRoomID; // Para qual sala vai
    public string targetSpawnKey; // Onde o player vai nascer na próxima sala
    public KeyCode customKey; // Barra de espaço ou outra tecla customizada
    public string description; // Ex: "Subir escadas" ou "Ir para esquerda"
}

[CreateAssetMenu(fileName = "NewRoom", menuName = "RPG/Room Data")]
public class RoomData : ScriptableObject
{
    public string roomID;
    public Sprite backgroundSprite;

    [Header("Limites da Tela")]
    public float minX;
    public float maxX;

    [Header("Ajuste do Player nesta Sala")]
    [Tooltip("Escala do personagem nesta sala (1 = normal, 0.7 = menor, 1.3 = maior)")]
    public float playerScale = 1f;
    [Tooltip("Marque se precisar ajustar a altura (Y) dos pés do personagem nesta sala")]
    public bool usarCustomY = false;
    public float playerY = -2.5f;

    [Header("Pontos de Entrada (Spawn)")]
    public List<SpawnPoint> spawnPoints;

    [Header("Gatilhos de Transição de Cena")]
    public List<TransitionTrigger> transitions;
}