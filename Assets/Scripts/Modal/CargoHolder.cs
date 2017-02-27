using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface CargoHolder
{

    Cargo getCargo();

    void setCargo(Cargo cargo);

    int getCargoCapacity();
}
