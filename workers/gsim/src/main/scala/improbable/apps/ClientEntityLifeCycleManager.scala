package improbable.apps
import com.typesafe.scalalogging.Logger
import improbable.natures.Player
import improbable.papi.EntityId
import improbable.papi.engine.EngineId
import improbable.papi.world.AppWorld
import improbable.papi.world.messaging.{EngineConnected, EngineDisconnected}
import improbable.papi.worldapp.WorldApp
import improbable.unity.fabric.engine.EnginePlatform

class ClientEntityLifeCycleManager(appWorld: AppWorld, logger: Logger) extends WorldApp {

  private var clientIdToEntityIdMap = Map[EngineId, EntityId]()

  appWorld.messaging.subscribe {
    case engineConnected: EngineConnected =>
      workerConnected(engineConnected)
    case engineDisconnected: EngineDisconnected =>
      workerDisconnected(engineDisconnected)
  }

  private def workerConnected(msg: EngineConnected): Unit = {
    msg match {
      case EngineConnected(clientId, EnginePlatform.UNITY_CLIENT_ENGINE, meta) =>
        val prefabToSpawn = "PlayerEnh"
//        if (!meta.isEmpty() && !meta.contains("{}")) {
//          val metaMap = meta.substring(1, meta.length - 1)
//            .split(",")
//            .map(_.split(":"))
//            .map { case Array(k, v) => (k.substring(1, k.length-1), v.substring(1, v.length-1))}
//            .toMap
//          if (metaMap.contains("ClientPrefab")) {
//            prefabToSpawn = metaMap.get("ClientPrefab").get
//          }
//        }
        spawnPlayer(clientId, prefabToSpawn)
      case _ =>
    }
  }

  private def spawnPlayer(clientId: EngineId, prefabToSpawn: String): Unit = {
    val playerEntityId = appWorld.entities.spawnEntity(Player(playerName = "Tolstoy", clientId, prefabToSpawn))
    logger.info(s"ClientId $clientId connected. Spawning a Player with entityId $playerEntityId")
    clientIdToEntityIdMap += clientId -> playerEntityId
  }

  private def workerDisconnected(msg: EngineDisconnected): Unit = {
    msg match {
      case EngineDisconnected(clientId, EnginePlatform.UNITY_CLIENT_ENGINE) =>
        deletePlayer(clientId)
      case _ =>
    }
  }

  private def deletePlayer(clientId: EngineId) = {
    clientIdToEntityIdMap.get(clientId) match {
      case Some(entityId) =>
        appWorld.entities.destroyEntity(entityId)
        logger.info(s"Client $clientId has disconnect. Destroyed entity $entityId")
      case None =>
        logger.warn(s"User disconnected but could not find entity id for player: $clientId")
    }
  }
}