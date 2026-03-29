using System;
using System.Collections.Generic;
using System.Linq;

// Sistema de batalla por turnos
public class BattleSystem
{
    // Lista de hormigas del jugador
    private List<Ant> players;

    // Lista de hormigas enemigas
    private List<Ant> enemies;

    // Constructor
    public BattleSystem(List<Ant> players, List<Ant> enemies)
    {
        this.players = players;
        this.enemies = enemies;
        StartBattle();
    }

    // Inicia la batalla
    private void StartBattle()
    {
        Console.WriteLine("La batalla ha comenzado!");
        NextTurn();
    }

    // Maneja el flujo de turnos
    private void NextTurn()
    {
        // Ordena todas las unidades vivas por velocidad
        var turnOrder = players.Concat(enemies)
            .Where(a => a.Stats.IsAlive())
            .OrderByDescending(a => a.Stats.Speed)
            .ToList();

        // Recorre cada unidad en orden
        foreach (var unit in turnOrder)
        {
            if (!unit.Stats.IsAlive()) continue;

            // Ejecuta acción según si es jugador o enemigo
            if (players.Contains(unit))
                ExecuteTurn(unit, enemies);
            else
                ExecuteTurn(unit, players);

            // Verifica si la batalla terminó
            if (CheckBattleEnd())
                return;
        }

        // Continúa la siguiente ronda
        NextTurn();
    }

    // Ejecuta el turno de una unidad
    private void ExecuteTurn(Ant attacker, List<Ant> targets)
    {
        // Selecciona un objetivo vivo
        Ant target = targets.FirstOrDefault(t => t.Stats.IsAlive());
        if (target == null) return;

        // Selecciona una habilidad según el rol
        Skill skill = ChooseSkill(attacker, target);

        // Ejecuta la habilidad
        skill.Use(attacker, target);

        Console.WriteLine($"{attacker.Name} usa {skill.Name} en {target.Name}");
    }

    // Selecciona la habilidad a usar según el rol
    private Skill ChooseSkill(Ant ant, Ant target)
    {
        switch (ant.Role)
        {
            case AntRole.Tank:
                // El tanque usa habilidades defensivas o básicas
                return ant.Skills[0];

            case AntRole.Fighter:
                // El luchador prioriza daño
                return ant.Skills[0];

            case AntRole.Support:
                // El soporte puede curar aliados
                return ant.Skills.Count > 1 ? ant.Skills[1] : ant.Skills[0];
        }

        return ant.Skills[0];
    }

    // Verifica si la batalla terminó
    private bool CheckBattleEnd()
    {
        // Si todos los jugadores murieron
        if (players.All(p => !p.Stats.IsAlive()))
        {
            Console.WriteLine("Perdiste");
            return true;
        }

        // Si todos los enemigos murieron
        if (enemies.All(e => !e.Stats.IsAlive()))
        {
            Console.WriteLine("Ganaste");
            return true;
        }

        return false;
    }
}