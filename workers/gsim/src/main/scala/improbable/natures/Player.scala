package improbable.natures
import improbable.corelib.natures.{BaseNature, NatureApplication, NatureDescription}
import improbable.corelibrary.transforms.TransformNature
import improbable.papi.entity.EntityPrefab
import improbable.papi.entity.behaviour.EntityBehaviourDescriptor
import improbable.player.{LocalPlayerCheck, Name, PlayerControls}
import improbable.behaviours._
import improbable.corelib.util.EntityOwner
import improbable.math.Vector3d
import improbable.papi.engine.EngineId
import improbable.sounds.{RadioControl, Sounds}

object Player extends NatureDescription {

  override val dependencies = Set[NatureDescription](BaseNature, TransformNature)

  override def activeBehaviours: Set[EntityBehaviourDescriptor] = {
    Set(descriptorOf[PrintNameBehaviour],
      descriptorOf[DelegateLocalPlayerCheckToClientBehaviour],
      descriptorOf[DelegatePlayerControlsToClientBehaviour],
      descriptorOf[DelegateSoundsToClientBehaviour],
      descriptorOf[RadioControlBehaviour],
      descriptorOf[RequestClientControlBehaviour]
    )
  }

  def apply(playerName: String, clientId: EngineId, prefabToSpawn: String): NatureApplication = {
    application(
      states = Seq(
        Name(playerName),
        EntityOwner(Some(clientId)),
        LocalPlayerCheck(),
        PlayerControls(
          Vector3d(0,0,0),
          Vector3d(0,0,0),
          Vector3d(0,0,0),
          Vector3d(0,0,0),
          Vector3d(0,0,0),
          Vector3d(0,0,0)),
        Sounds(),
        RadioControl(-1)
      ),
      natures = Seq(
        BaseNature(entityPrefab = EntityPrefab(prefabToSpawn), isPhysical = true),
        TransformNature()
      )
    )
  }
}