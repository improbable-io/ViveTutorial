package improbable.natures

import improbable.behaviours.{DelegateLocalPlayerCheckToClientBehaviour, PrintNameBehaviour}
import improbable.corelib.natures.{BaseNature, NatureApplication, NatureDescription}
import improbable.corelib.util.EntityOwner
import improbable.corelibrary.transforms.TransformNature
import improbable.papi.engine.EngineId
import improbable.papi.entity.EntityPrefab
import improbable.papi.entity.behaviour.EntityBehaviourDescriptor
import improbable.player.Name

object Player extends NatureDescription {

  override val dependencies = Set[NatureDescription](BaseNature, TransformNature)

  override def activeBehaviours: Set[EntityBehaviourDescriptor] = {
    Set(
      descriptorOf[PrintNameBehaviour],
      descriptorOf[DelegateLocalPlayerCheckToClientBehaviour]
    )
  }

  def apply(playerName: String, clientId: EngineId): NatureApplication = {
    application(
      states = Seq(
        Name(playerName),
        EntityOwner(Some(clientId))
      ),
      natures = Seq(
        BaseNature(entityPrefab = EntityPrefab("Player"), isPhysical = true),
        TransformNature()
      )
    )
  }
}