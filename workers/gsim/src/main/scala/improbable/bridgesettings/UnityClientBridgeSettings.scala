package improbable.bridgesettings

import improbable.corelib.BridgeSettings.UnityClientAssetContextDiscriminator
import improbable.fapi.bridge.{BridgeSettings, BridgeSettingsResolver, ConstantEngineLoadPolicy, PerEntityOrderedStateUpdateQos}
import improbable.fapi.network.RakNetLinkSettings
import improbable.unity.fabric.AuthoritativeEntityOnly
import improbable.unity.fabric.engine.EnginePlatform
import improbable.unity.fabric.satisfiers.SatisfyVisual

object UnityClientBridgeSettings extends BridgeSettingsResolver {

  private val clientEngineBridgeSettings = BridgeSettings(
    assetContextDiscriminator = UnityClientAssetContextDiscriminator(),
    linkSettings = RakNetLinkSettings(),
    enginePlatform = EnginePlatform.UNITY_CLIENT_ENGINE,
    constraintSatisfier = SatisfyVisual,
    entityInterestPolicy = AuthoritativeEntityOnly(),
    engineLoadPolicy = ConstantEngineLoadPolicy(0.5),
    stateUpdateQos = PerEntityOrderedStateUpdateQos
  )

  override def engineTypeToBridgeSettings(engineType: String, metadata: String): Option[BridgeSettings] = {
    if (engineType == EnginePlatform.UNITY_CLIENT_ENGINE) {
      Some(clientEngineBridgeSettings)
    } else {
      None
    }
  }
}
