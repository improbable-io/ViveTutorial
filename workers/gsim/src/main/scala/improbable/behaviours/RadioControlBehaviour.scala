package improbable.behaviours

import improbable.corelib.util.EntityOwnerDelegation.entityOwnerDelegation
import improbable.math.Coordinates
import improbable.natures.Radio
import improbable.papi.entity.{Entity, EntityBehaviour}
import improbable.papi.world.World
import improbable.sounds.{RadioControl, RadioControlWriter}

class RadioControlBehaviour(entity: Entity, radioControlWriter: RadioControlWriter, world: World) extends EntityBehaviour {

  override def onReady() {
    entity.delegateStateToOwner[RadioControl]

    entity.watch[RadioControl].onTurnOn {
      turnOnRadioData =>
        if (radioControlWriter.radioId == -1) {
          val radioId = world.entities.spawnEntity(Radio(Coordinates(turnOnRadioData.position)))
          radioControlWriter.update.radioId(radioId).finishAndSend()
        }
    }

    entity.watch[RadioControl].onTurnOff {
      turnOffRadioData =>
        world.entities.destroyEntity(radioControlWriter.radioId)
        radioControlWriter.update.radioId(-1).finishAndSend()
    }
  }
}