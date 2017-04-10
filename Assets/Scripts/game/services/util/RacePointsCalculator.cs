using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/**
 * Utility class to calculate the advantage points for a race.
 */
public class RacePointsCalculator
{
    
    private static Dictionary<LRT, int> lrtPointCost;
    private static Dictionary<PRT, int> prtPointCost;

    static RacePointsCalculator() {
        lrtPointCost = new Dictionary<LRT, int>();
        prtPointCost = new Dictionary<PRT, int>();

        lrtPointCost.Add(LRT.IFE, -235);
        lrtPointCost.Add(LRT.TT, -25);
        lrtPointCost.Add(LRT.ARM, -159);
        lrtPointCost.Add(LRT.ISB, -201);
        lrtPointCost.Add(LRT.GR, 40);
        lrtPointCost.Add(LRT.UR, -240);
        lrtPointCost.Add(LRT.MA, -155);
        lrtPointCost.Add(LRT.NRSE, 160);
        lrtPointCost.Add(LRT.CE, 240);
        lrtPointCost.Add(LRT.OBRM, 255);
        lrtPointCost.Add(LRT.NAS, 325);
        lrtPointCost.Add(LRT.LSP, 180);
        lrtPointCost.Add(LRT.BET, 70);
        lrtPointCost.Add(LRT.RS, 30);

        prtPointCost.Add(PRT.HE, -40);
        prtPointCost.Add(PRT.SS, -95);
        prtPointCost.Add(PRT.WM, -45);
        prtPointCost.Add(PRT.CA, -10);
        prtPointCost.Add(PRT.IS, 100);
        prtPointCost.Add(PRT.SD, 150);
        prtPointCost.Add(PRT.PP, -120);
        prtPointCost.Add(PRT.IT, -180);
        prtPointCost.Add(PRT.AR, -90);
        prtPointCost.Add(PRT.JoaT, 66);
    }

private static Race race;
private static int TTCorrectionFactor;
private static int numIterationsGrav;
private static int numIterationsRad;
private static int numIterationsTemp;
private static Hab testPlanetHab = new Hab();

/*
 * Get the Advantage points for a race
 */
public static int getPointsAdvantage(Race race)
{
    RacePointsCalculator.race = race;
    race.init();
        
    int points = Consts.raceStartingPoints;

    int habPoints = (int)(getHabRange() / 2000);

    int growthRateFactor = race.getGrowthRate(); 

    float grRate = growthRateFactor;
        
    if (growthRateFactor <= 5)
    {
        points += (6 - growthRateFactor) * 4200;
    }
    else if (growthRateFactor <= 13)
    {
        switch (growthRateFactor)
        {
            case 6:
                points += 3600;
                break;
            case 7:
                points += 2250;
                break;
            case 8:
                points += 600;
                break;
            case 9:
                points += 225;
                break;
        }
        growthRateFactor = growthRateFactor * 2 - 5;
    }
    else if (growthRateFactor < 20)
    {
        growthRateFactor = (growthRateFactor - 6) * 3;
    }
    else
    {
        growthRateFactor = 45;
    }

    points -= (int)(habPoints * growthRateFactor) / 24;
        
    int numImmunities = 0;
    for (int habType = 0; habType < 3; habType++)
    {
        if (race.isImmune(habType))
        {
            numImmunities++;
        }
        else
        {
            points += Mathf.Abs(race.getHabCenter(habType) - 50) * 4;
        }
    }
    
    if (numImmunities > 1)
    {
        points -= 150;
    }
    
    int operationPoints = race.getNumFactories();
    int productionPoints = race.getFactoryOutput();

    if (operationPoints > 10 || productionPoints > 10)
    {
        operationPoints -= 9;
        if (operationPoints < 1)
        {
            operationPoints = 1;
        }
        productionPoints -= 9;
        if (productionPoints < 1)
        {
            productionPoints = 1;
        }
        
        int factoryProductionCost = 2;
        if (race.getPRT() == PRT.HE)
        {
            factoryProductionCost = 3;
        }

        productionPoints *= factoryProductionCost;
            
        if (numImmunities >= 2)
        {
            points -= (int)((productionPoints * operationPoints) * grRate) / 2;
        }
        else
        {
            points -= (int)((productionPoints * operationPoints) * grRate) / 9;
        }
    }
    
    int popEfficiency = race.getColonistsPerResource() / 100;
    if (popEfficiency > 25)
        popEfficiency = 25;

    if (popEfficiency <= 7)
        points -= 2400;
    else if (popEfficiency == 8)
        points -= 1260;
    else if (popEfficiency == 9)
        points -= 600;
    else if (popEfficiency > 10)
        points += (popEfficiency - 10) * 120;
    
    if (race.getPRT() == PRT.AR)
    {
        points += 210;
    }
    else
    {
        productionPoints = 10 - race.getFactoryOutput();
        int costPoints = 10 - race.getFactoryCost();
        operationPoints = 10 - race.getNumFactories();
        int tmpPoints = 0;

        if (productionPoints > 0)
        {
            tmpPoints = productionPoints * 100;
        }
        else
        {
            tmpPoints = productionPoints * 121;
        }

        if (costPoints > 0)
        {
            tmpPoints += costPoints * costPoints * -60;
        }
        else
        {
            tmpPoints += costPoints * -55;
        }

        if (operationPoints > 0)
        {
            tmpPoints += operationPoints * 40;
        }
        else
        {
            tmpPoints += operationPoints * 35;
        }
        
        int llfp = 700;
        if (tmpPoints > llfp)
        {
            tmpPoints = (tmpPoints - llfp) / 3 + llfp;
        }

        if (operationPoints <= -7)
        {
            if (operationPoints < -11)
            {
                if (operationPoints < -14)
                {
                    tmpPoints -= 360;
                }
                else
                {
                    tmpPoints += (operationPoints + 7) * 45;
                }
            }
            else
            {
                tmpPoints += (operationPoints + 6) * 30;
            }
        }

        if (productionPoints <= -3)
        {
            tmpPoints += (productionPoints + 2) * 60;
        }

        points += tmpPoints;

        if (race.isFactoriesCostLess())
        {
            points -= 175;
        }
        productionPoints = 10 - race.getMineOutput();
        costPoints = 3 - race.getMineCost();
        operationPoints = 10 - race.getNumMines();
        tmpPoints = 0;

        if (productionPoints > 0)
        {
            tmpPoints = productionPoints * 100;
        }
        else
        {
            tmpPoints = productionPoints * 169;
        }

        if (costPoints > 0)
        {
            tmpPoints -= 360;
        }
        else
        {
            tmpPoints += costPoints * (-65) + 80;
        }

        if (operationPoints > 0)
        {
            tmpPoints += operationPoints * 40;
        }
        else
        {
            tmpPoints += operationPoints * 35;
        }

        points += tmpPoints;
    }
    
    points += prtPointCost[race.getPRT()];

    // too many lrts
    int badLRTs = 0;
    int goodLRTs = 0;
        
    foreach (LRT lrt in race.getLrts())
    {
        if (lrtPointCost[lrt] >= 0)
        {
            badLRTs++;
        }
        else
        {
            goodLRTs++;
        }
        points += lrtPointCost[lrt];
    }

    if (goodLRTs + badLRTs > 4)
    {
        points -= (goodLRTs + badLRTs) * (goodLRTs + badLRTs - 4) * 10;
    }
    if (badLRTs - goodLRTs > 3)
    {
        points -= (badLRTs - goodLRTs - 3) * 60;
    }
    if (goodLRTs - badLRTs > 3)
    {
        points -= (goodLRTs - badLRTs - 3) * 40;
    }
    
    if (race.getLrts().Contains(LRT.NAS))
    {
        if (race.getPRT() == PRT.PP)
        {
            points -= 280;
        }
        else if (race.getPRT() == PRT.SS)
        {
            points -= 200;
        }
        else if (race.getPRT() == PRT.JoaT)
        {
            points -= 40;
        }
    }
    
    int techcosts = 0;
    for (int i = 0; i < 6; i++)
    {
        ResearchCostLevel rc = race.getResearchCost().getAtIndex(i);
        if (rc == ResearchCostLevel.Extra)
        {
            techcosts--;
        }
        else if (rc == ResearchCostLevel.Less)
        {
            techcosts++;
        }
    }
    if (techcosts > 0)
    {
        points -= (techcosts * techcosts) * 130;
        if (techcosts >= 6)
        {
            points += 1430;
        }
        else if (techcosts == 5)
        {
            points += 520;
        }
    }
    else if (techcosts < 0)
    {
        int[] scienceCost = new int[] { 150, 330, 540, 780, 1050, 1380 };
        points += scienceCost[(-techcosts) - 1];
        if (techcosts < -4 && race.getColonistsPerResource() < 1000)
        {
            points -= 190;
        }
    }

    if (race.isTechsStartHigh())
    {
        points -= 180;
    }
    
    if (race.getPRT() == PRT.AR && race.getResearchCost().getEnergy() == ResearchCostLevel.Less)
    {
        points -= 100;
    }

    return points / 3;

}

/**
 * Compute the hab range advantage points for this race by generating test planets for a variety
 * of ranges and using the habitability of those planets
 */
private static long getHabRange()
{
    bool totalTerraforming;
    double temperatureSum, gravitySum;
    long radiationSum, planetDesirability;
    int terraformOffsetSum, tmpHab;
    int[] terraformOffset = new int[3];
        
    Hab testHabStart = new Hab();
    Hab testHabWidth = new Hab();

    double points = 0.0;
    totalTerraforming = race.getLrts().Contains(LRT.TT);

    terraformOffset[0] = terraformOffset[1] = terraformOffset[2] = 0;
        
    if (race.isImmuneGrav())
    {
        numIterationsGrav = 1;
    }
    else
    {
        numIterationsGrav = 11;
    }
    if (race.isImmuneTemp())
    {
        numIterationsTemp = 1;
    }
    else
    {
        numIterationsTemp = 11;
    }
    if (race.isImmuneRad())
    {
        numIterationsRad = 1;
    }
    else
    {
        numIterationsRad = 11;
    }
    for (int loopIndex = 0; loopIndex < 3; loopIndex++)
    {

        // each main loop gets a different TTCorrectionFactor
        if (loopIndex == 0)
            TTCorrectionFactor = 0;
        else if (loopIndex == 1)
            TTCorrectionFactor = totalTerraforming ? 8 : 5;
        else
            TTCorrectionFactor = totalTerraforming ? 17 : 15;

        
        for (int habType = 0; habType < 3; habType++)
        {
            if (race.isImmune(habType))
            {
                testHabStart.setAtIndex(habType, 50);
                testHabWidth.setAtIndex(habType, 11);

            }
            else
            {
                testHabStart.setAtIndex(habType, race.getHabLow().getAtIndex(habType) - TTCorrectionFactor);
                    
                if (testHabStart.getAtIndex(habType) < 0)
                {
                    testHabStart.setAtIndex(habType, 0);
                }
                
                tmpHab = race.getHabHigh().getAtIndex(habType) + TTCorrectionFactor;
                    
                if (tmpHab > 100)
                    tmpHab = 100;
                
                testHabWidth.setAtIndex(habType, tmpHab - testHabStart.getAtIndex(habType));
            }
        }
        gravitySum = 0.0;
        for (int iterationGrav = 0; iterationGrav < numIterationsGrav; iterationGrav++)
        {
            tmpHab = getPlanetHabForHabIndex(iterationGrav, 0, loopIndex, numIterationsGrav, testHabStart.getGrav(), testHabWidth.getGrav(), terraformOffset);
            testPlanetHab.setGrav(tmpHab);
                
            temperatureSum = 0.0;
            for (int iterationTemp = 0; iterationTemp < numIterationsTemp; iterationTemp++)
            {
                tmpHab = getPlanetHabForHabIndex(iterationTemp, 1, loopIndex, numIterationsTemp, testHabStart.getTemp(), testHabWidth.getTemp(), terraformOffset);
                testPlanetHab.setTemp(tmpHab);
                    
                radiationSum = 0;
                for (int iterationRad = 0; iterationRad < numIterationsRad; iterationRad++)
                {
                    tmpHab = getPlanetHabForHabIndex(iterationRad, 2, loopIndex, numIterationsRad, testHabStart.getRad(), testHabWidth.getRad(), terraformOffset);
                    testPlanetHab.setRad(tmpHab);

                    planetDesirability = race.getPlanetHabitability(testPlanetHab);

                    terraformOffsetSum = terraformOffset[0] + terraformOffset[1] + terraformOffset[2];
                    if (terraformOffsetSum > TTCorrectionFactor)
                    {
                        planetDesirability -= terraformOffsetSum - TTCorrectionFactor;
                        if (planetDesirability < 0)
                            planetDesirability = 0;
                    }
                    planetDesirability *= planetDesirability;
                        
                    switch (loopIndex)
                    {
                        case 0:
                            planetDesirability *= 7;
                            break;
                        case 1:
                            planetDesirability *= 5;
                            break;
                        default:
                            planetDesirability *= 6;
                            break;
                    }

                    radiationSum += planetDesirability;
                }
                if (!race.isImmuneRad())
                {
                    radiationSum = (radiationSum * testHabWidth.getRad()) / 100;
                }
                else
                {
                    radiationSum *= 11;
                }

                temperatureSum += radiationSum;
            }
            if (!race.isImmuneTemp())
            {
                temperatureSum = (temperatureSum * testHabWidth.getTemp()) / 100;
            }
            else
            {
                temperatureSum *= 11;
            }

            gravitySum += temperatureSum;
        }
        if (!race.isImmuneGrav())
        {
            gravitySum = (gravitySum * testHabWidth.getGrav()) / 100;
        }
        else
        {
            gravitySum *= 11;
        }

        points += gravitySum;
    }

    return (long)(points / 10.0 + 0.5);
}

/*
 * Get the planet hab value (grav, temp or rad) for an iteration of the loop
 */
private static int getPlanetHabForHabIndex(int iterIndex, int habType, int loopIndex, int numIterations, int testHabStart, int testHabWidth,
                                    int[] terraformOffset)
{
    int tmpHab = 0;
        
    if (iterIndex == 0 || numIterations <= 1)
    {
        tmpHab = testHabStart;
    }
    else
    {
        tmpHab = (testHabWidth * iterIndex) / (numIterations - 1) + testHabStart;
    }
    
    if (loopIndex != 0 && !race.isImmune(habType))
    {
        int offset = race.getHabCenter(habType) - tmpHab;
        if (Mathf.Abs(offset) <= TTCorrectionFactor)
        {
            offset = 0;
        }
        else if (offset < 0)
        {
            offset += TTCorrectionFactor;
        }
        else
        {
            offset -= TTCorrectionFactor;
        }
        
        terraformOffset[habType] = offset;
        tmpHab = race.getHabCenter(habType) - offset;
    }

    return tmpHab;
}
    
}
