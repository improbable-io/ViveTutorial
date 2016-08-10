package improbable.natures
import improbable.corelib.natures.{BaseNature, NatureApplication, NatureDescription}
import improbable.corelibrary.transforms.TransformNature
import improbable.papi.entity.EntityPrefab
import improbable.papi.entity.behaviour.EntityBehaviourDescriptor
import improbable.player.{LocalPlayerCheck, Name, PlayerControls}
import improbable.behaviours.{DelegateLocalPlayerCheckToClientBehaviour, DelegatePlayerControlsToClientBehaviour, PrintNameBehaviour}
import improbable.corelib.util.EntityOwner
import improbable.math.Vector3d
import improbable.papi.engine.EngineId

object Player extends NatureDescription {

  override val dependencies = Set[NatureDescription](BaseNature, TransformNature)

  override def activeBehaviours: Set[EntityBehaviourDescriptor] = {
    Set(descriptorOf[PrintNameBehaviour],
      descriptorOf[DelegateLocalPlayerCheckToClientBehaviour],
      descriptorOf[DelegatePlayerControlsToClientBehaviour]
    )
  }

  def apply(playerName: String, clientId: EngineId): NatureApplication = {
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
          Vector3d(0,0,0))
      ),
      natures = Seq(
        BaseNature(entityPrefab = EntityPrefab("Player"), isPhysical = true),
        TransformNature()
      )
    )
  }
}