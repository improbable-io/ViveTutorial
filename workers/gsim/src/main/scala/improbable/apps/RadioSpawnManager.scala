package improbable.apps

import improbable.math.Coordinates
import improbable.natures.Radio
import improbable.papi.world.AppWorld
import improbable.papi.worldapp.WorldApp

class RadioSpawnManager(appWorld: AppWorld) extends WorldApp {
  appWorld.entities.spawnEntity(Radio(Coordinates(5,0,0)))
}
