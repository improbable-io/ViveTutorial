package improbable.behaviours

import com.typesafe.scalalogging.Logger
import improbable.corelibrary.transforms.TransformInterface
import improbable.papi.entity.{Entity, EntityBehaviour}
import improbable.papi.world.World
import improbable.papi.world.messaging.CustomMsg

import scala.concurrent.duration._


case class CheckHeartBeat() extends CustomMsg

class HandleClientControlBehaviour(entity: Entity, world: World, transformInterface: TransformInterface, logger: Logger) extends EntityBehaviour {

  var receivedHeartbeat = false

  override def onReady() {

    /* If we cross a GSim boundary world.timing stops working
       So this reschedules the heartbeat in the event that
       an entity moves over the GSim boundary while allocated
       to a unity client rather than a Physics engine */
    if (!transformInterface.isPhysicsDelegatedToUnityServer) {
      scheduleHeartbeat()
    }

    world.messaging.onReceive {
      case DelegateToPlayer(clientId) =>
        transformInterface.delegatePhysicsToClient(clientId)
        logger.info(s"Delegated physics of entity ${entity.entityId} to client $clientId")
        receivedHeartbeat = true
        scheduleHeartbeat()

      case HeartbeatFromPlayer(clientId) =>
        logger.info(s"Got a heartbeat for  ${entity.entityId} from client $clientId")
        receivedHeartbeat = true

      case CheckHeartBeat() =>
        if (receivedHeartbeat) {
          receivedHeartbeat = false
          scheduleHeartbeat()
        } else {
          transformInterface.delegatePhysicsToUnityServer()
          logger.info(s"Delegated physics of entity ${entity.entityId} back to physics worker")
        }


      case _ =>
    }
  }

  def scheduleHeartbeat(): Unit = {
    //Send a message to ourselves
    world.timing.after(5.seconds){
      world.messaging.sendToEntity(entity.entityId, CheckHeartBeat())
    }
  }

}