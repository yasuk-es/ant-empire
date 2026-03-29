using System.Collections.Generic;
using UnityEngine;

// Manager principal del juego (controla la inicialización)
public class GameManager : MonoBehaviour
{
    // Listas de unidades
    private List<Ant> playerTeam;
    private List<Ant> enemyTeam;

    void Start()
    {
        // Se crean los equipos al iniciar el juego
        CreateTeams();

        // Se inicia el sistema de batalla
        BattleSystem battle = new BattleSystem(playerTeam, enemyTeam);
    }

    // Método que crea los equipos
    void CreateTeams()
    {
        // =========================
        // 🐜 EQUIPO DEL JUGADOR
        // =========================
        playerTeam = new List<Ant>();

        // 🛡️ TANQUE
        Ant tank = new Ant(
            "Guardiana",
            "Defensora",
            AntRole.Tank,
            new AntStats(150, 20, 15, 5),
            new List<Skill>()
            {
                new BasicAttack(), // siempre primer skill
                new StunAttack()
            }
        );

        // ⚔️ LUCHADOR
        Ant fighter = new Ant(
            "Soldado",
            "Atacante",
            AntRole.Fighter,
            new AntStats(100, 30, 5, 10),
            new List<Skill>()
            {
                new BasicAttack(),
                new DoubleStrike()
            }
        );

        // 💚 SOPORTE
        Ant support = new Ant(
            "Sanadora",
            "Soporte",
            AntRole.Support,
            new AntStats(90, 15, 8, 12),
            new List<Skill>()
            {
                new BasicAttack(),
                new HealSkill()
            }
        );

        // Se agregan al equipo del jugador
        playerTeam.Add(tank);
        playerTeam.Add(fighter);
        playerTeam.Add(support);

        // =========================
        // 👾 EQUIPO ENEMIGO (simple)
        // =========================
        enemyTeam = new List<Ant>();

        // Enemigos básicos (pueden ser clones por ahora)
        Ant enemy1 = new Ant(
            "Enemigo 1",
            "Hostil",
            AntRole.Fighter,
            new AntStats(100, 25, 5, 8),
            new List<Skill>()
            {
                new BasicAttack(),
                new DoubleStrike()
            }
        );

        Ant enemy2 = new Ant(
            "Enemigo 2",
            "Hostil",
            AntRole.Tank,
            new AntStats(140, 18, 12, 6),
            new List<Skill>()
            {
                new BasicAttack(),
                new StunAttack()
            }
        );

        enemyTeam.Add(enemy1);
        enemyTeam.Add(enemy2);
    }
}