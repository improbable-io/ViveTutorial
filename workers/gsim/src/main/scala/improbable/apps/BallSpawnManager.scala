package improbable.apps

import improbable.math.Coordinates
import improbable.natures.Ball
import improbable.papi.world.AppWorld
import improbable.papi.worldapp.WorldApp

class BallSpawnManager(appWorld: AppWorld) extends WorldApp {

  for {
    i <- 1 to 5
    j <- 1 to 5
  } {
    appWorld.entities.spawnEntity(Ball(Coordinates(i,10,j)))
  }

}
