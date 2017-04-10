using System;
using System.Collections.Generic;
using UnityEngine;

public class Consts {

    public static int maxTechLevel = 26;

    public static int startingYear = 2400;
    public static int startingPopulation = 400000; //25000;
    public static int minStartingConc = 3;
    public static int minMineralConc = 1;
    public static int minHWMineralConc = 30;
    public static int maxStartingConc = 200;
    public static int maxStartingSurf = 1000;
    public static int minStartingSurf = 300;
    public static int startingDefenses = 10;
    public static int startingFactories = 10;
    public static int startingMines = 10;
    public static int factoryCostGermanium = 4;
    public static Cost defenseCost = new Cost(5, 5, 5, 15);
    public static Cost terraformCost = new Cost(200, 200, 200, 50);
    public static float lowStartingPopFactor = .7f;
    public static int mineralAlchemyCost = 100;
    public static int mineralAlchemyLRTCost = 25;
    public static int mineralDecayFactor = 1500000;
    public static int raceStartingPoints = 1650;
    public static int startingTechLevelExtra = 3;
    public static int startingTechLevelJoaT = 3;
    public static int startingTechLevelExtra_joat = 4;
    public static int planetMinDistance = 35;
    public static int builtInScannerJoaTMultiplier = 20;

    public static string[] aiNames = new string[] { "Rogue", "Phantom", "K2SO", "BB8", "C3PO", "R2D2" };

    /**
     * The amount techs cost based on research cost
     */
    public static int[] techResearchCost;
    
    public static Dictionary<Size, int> sizeToArea = new Dictionary<Size, int>();
    
    public static Dictionary<Size, Dictionary<Density, int>> sizeToDensity = new Dictionary<Size, Dictionary<Density, int>>();

    static Consts() {

        techResearchCost = new int[] { 0,
                                      50,
                                      80,
                                      130,
                                      210,
                                      340,
                                      550,
                                      890,
                                      1440,
                                      2330,
                                      3770,
                                      6100,
                                      9870,
                                      13850,
                                      18040,
                                      22440,
                                      27050,
                                      31870,
                                      36900,
                                      42140,
                                      47590,
                                      53250,
                                      59120,
                                      65200,
                                      71490,
                                      77990,
                                      84700,
                                      int.MaxValue
};

        
        sizeToArea.Add(Size.Tiny, 400);
        sizeToArea.Add(Size.Small, 800);
        sizeToArea.Add(Size.Medium, 1200);
        sizeToArea.Add(Size.Large, 1600);
        sizeToArea.Add(Size.Huge, 2000);

        foreach (Size size in Enum.GetValues(typeof(Size))) {
            Dictionary<Density, int> densityToPlanets = new Dictionary<Density, int>();

            switch (size) {
            case Size.Huge:
                densityToPlanets.Add(Density.Sparse, 600);
                densityToPlanets.Add(Density.Normal, 800);
                densityToPlanets.Add(Density.Dense, 940);
                densityToPlanets.Add(Density.Packed, 945);
                break;
            case Size.Large:
                densityToPlanets.Add(Density.Sparse, 384);
                densityToPlanets.Add(Density.Normal, 512);
                densityToPlanets.Add(Density.Dense, 640);
                densityToPlanets.Add(Density.Packed, 910);
                break;
            case Size.Medium:
                densityToPlanets.Add(Density.Sparse, 216);
                densityToPlanets.Add(Density.Normal, 288);
                densityToPlanets.Add(Density.Dense, 360);
                densityToPlanets.Add(Density.Packed, 540);
                break;
            case Size.Small:
                densityToPlanets.Add(Density.Sparse, 96);
                densityToPlanets.Add(Density.Normal, 128);
                densityToPlanets.Add(Density.Dense, 160);
                densityToPlanets.Add(Density.Packed, 240);
                break;
            case Size.Tiny:
                densityToPlanets.Add(Density.Sparse, 24);
                densityToPlanets.Add(Density.Normal, 32);
                densityToPlanets.Add(Density.Dense, 40);
                densityToPlanets.Add(Density.Packed, 60);
                break;
            }

            sizeToDensity.Add(size, densityToPlanets);

        }

    }

}
