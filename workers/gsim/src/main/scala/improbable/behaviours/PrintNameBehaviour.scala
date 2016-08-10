package improbable.behaviours
import improbable.papi.entity.{Entity, EntityBehaviour}
import com.typesafe.scalalogging.Logger
import improbable.papi.world.World
import improbable.player.Name

class PrintNameBehaviour(entity: Entity, world: World, logger: Logger) extends EntityBehaviour {

  private val nameWatcher = entity.watch[Name]

  override def onReady(): Unit = {
    logger.info(s"Entity ${entity.entityId} with name ${nameWatcher.value.get} has just been spawned")
  }
}