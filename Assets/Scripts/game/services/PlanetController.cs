public interface PlanetController
{

    /**
     * Create a new planet
     */
    Planet makePlanet(string name, int x, int y);

    /**
     * Randomize a planet
     */
    void randomise(Planet planet);

    /**
     * Grow a planet by whatever amount it grows in a year
     */
    void managePopulation(Planet planet);

    /**
     * Mine a planet, moving minerals from concentrations to surface minerals
     */
    void mine(Planet planet);

    /**
     * Build anything in the production queue on the planet.
     */
    int buildAndProduce(Planet planet);

}