using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAnimationInput 
{
     void SetInitialData(GetRead getRead, List<PhenotypeData> data = null);

    void Run();
    void End();
  
    

}
