public interface PlanetController
{

    /**
     * Create a new planet
     * @param name The name of the planet
     * @param x The x coord of the planet
     * @param y The y coord of the planet
     * @return The newly created planet
     */
    Planet create(string name, int x, int y);

    /**
     * Randomize a planet
     * @param planet the planet to randomize
     */
    void randomize(Planet planet);

    /**
     * Grow a planet by whatever amount it grows in a year
     * @param planet The planet to grow
     */
    void grow(Planet planet);

    /**
     * Mine a planet, moving minerals from concentrations to to surface minerals
     * @param planet the  planet to mine
     */
    void mine(Planet planet);

    /**
     * Build anything in the production queue on the planet.
     * @param planet The planet to build.
     * @return The number of resources left over after building, used for research
     */
    int build(Planet planet);

}