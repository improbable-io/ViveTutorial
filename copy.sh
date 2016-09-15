#!/bin/sh
FILES=('README.md' 'workers/gsim/src/main/scala/improbable/natures/Player.scala' 'workers/unity/Assets/ClientScene.unity' 'workers/unity/Assets/EntityPrefabs/Player.prefab' 'workers/unity/Assets/EntityPrefabs/Player.prefab.meta' 'workers/unity/ProjectSettings/ProjectSettings.asset' '.protocache/' 'schema/improbable/player/player_controls.schema' 'workers/gsim/src/main/scala/improbable/behaviours/DelegatePlayerControlsToClientBehaviour.scala' 'workers/unity/Assets/CameraRigEnabler.cs' 'workers/unity/Assets/CameraRigEnabler.cs.meta' 'workers/unity/Assets/InputReceiver.cs' 'workers/unity/Assets/InputReceiver.cs.meta' 'workers/unity/Assets/InputSender.cs' 'workers/unity/Assets/InputSender.cs.meta' 'workers/unity/Assets/LocalPlayerModelSynchronizer.cs' 'workers/unity/Assets/LocalPlayerModelSynchronizer.cs.meta' 'workers/unity/Assets/ModelReceiver.cs' 'workers/unity/Assets/ModelReceiver.cs.meta' 'workers/unity/Assets/ModelSender.cs' 'workers/unity/Assets/ModelSender.cs.meta' 'workers/unity/Assets/Plugins/' 'workers/unity/Assets/SteamVR.meta' 'workers/unity/Assets/SteamVR/')
COUNT=${#FILES[@]}

for (( i=0;i<$COUNT;i++)); do
        FILE=${FILES[${i}]}
        DEST='../ViveTutorial/'$(dirname $FILE)
        cp -r $FILE $DEST
done
