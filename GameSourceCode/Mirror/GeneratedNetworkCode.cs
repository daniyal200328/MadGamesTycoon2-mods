using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Mirror;

[StructLayout(LayoutKind.Auto, CharSet = CharSet.Auto)]
public static class GeneratedNetworkCode
{
	public static TimeSnapshotMessage _Read_Mirror_002ETimeSnapshotMessage(NetworkReader reader)
	{
		return default(TimeSnapshotMessage);
	}

	public static void _Write_Mirror_002ETimeSnapshotMessage(NetworkWriter writer, TimeSnapshotMessage value)
	{
	}

	public static ReadyMessage _Read_Mirror_002EReadyMessage(NetworkReader reader)
	{
		return default(ReadyMessage);
	}

	public static void _Write_Mirror_002EReadyMessage(NetworkWriter writer, ReadyMessage value)
	{
	}

	public static NotReadyMessage _Read_Mirror_002ENotReadyMessage(NetworkReader reader)
	{
		return default(NotReadyMessage);
	}

	public static void _Write_Mirror_002ENotReadyMessage(NetworkWriter writer, NotReadyMessage value)
	{
	}

	public static AddPlayerMessage _Read_Mirror_002EAddPlayerMessage(NetworkReader reader)
	{
		return default(AddPlayerMessage);
	}

	public static void _Write_Mirror_002EAddPlayerMessage(NetworkWriter writer, AddPlayerMessage value)
	{
	}

	public static SceneMessage _Read_Mirror_002ESceneMessage(NetworkReader reader)
	{
		return new SceneMessage
		{
			sceneName = reader.ReadString(),
			sceneOperation = _Read_Mirror_002ESceneOperation(reader),
			customHandling = reader.ReadBool()
		};
	}

	public static SceneOperation _Read_Mirror_002ESceneOperation(NetworkReader reader)
	{
		return (SceneOperation)NetworkReaderExtensions.ReadByte(reader);
	}

	public static void _Write_Mirror_002ESceneMessage(NetworkWriter writer, SceneMessage value)
	{
		writer.WriteString(value.sceneName);
		_Write_Mirror_002ESceneOperation(writer, value.sceneOperation);
		writer.WriteBool(value.customHandling);
	}

	public static void _Write_Mirror_002ESceneOperation(NetworkWriter writer, SceneOperation value)
	{
		NetworkWriterExtensions.WriteByte(writer, (byte)value);
	}

	public static CommandMessage _Read_Mirror_002ECommandMessage(NetworkReader reader)
	{
		return new CommandMessage
		{
			netId = reader.ReadUInt(),
			componentIndex = NetworkReaderExtensions.ReadByte(reader),
			functionHash = reader.ReadUShort(),
			payload = reader.ReadBytesAndSizeSegment()
		};
	}

	public static void _Write_Mirror_002ECommandMessage(NetworkWriter writer, CommandMessage value)
	{
		writer.WriteUInt(value.netId);
		NetworkWriterExtensions.WriteByte(writer, value.componentIndex);
		writer.WriteUShort(value.functionHash);
		writer.WriteBytesAndSizeSegment(value.payload);
	}

	public static RpcMessage _Read_Mirror_002ERpcMessage(NetworkReader reader)
	{
		return new RpcMessage
		{
			netId = reader.ReadUInt(),
			componentIndex = NetworkReaderExtensions.ReadByte(reader),
			functionHash = reader.ReadUShort(),
			payload = reader.ReadBytesAndSizeSegment()
		};
	}

	public static void _Write_Mirror_002ERpcMessage(NetworkWriter writer, RpcMessage value)
	{
		writer.WriteUInt(value.netId);
		NetworkWriterExtensions.WriteByte(writer, value.componentIndex);
		writer.WriteUShort(value.functionHash);
		writer.WriteBytesAndSizeSegment(value.payload);
	}

	public static RpcBufferMessage _Read_Mirror_002ERpcBufferMessage(NetworkReader reader)
	{
		return new RpcBufferMessage
		{
			payload = reader.ReadBytesAndSizeSegment()
		};
	}

	public static void _Write_Mirror_002ERpcBufferMessage(NetworkWriter writer, RpcBufferMessage value)
	{
		writer.WriteBytesAndSizeSegment(value.payload);
	}

	public static SpawnMessage _Read_Mirror_002ESpawnMessage(NetworkReader reader)
	{
		return new SpawnMessage
		{
			netId = reader.ReadUInt(),
			isLocalPlayer = reader.ReadBool(),
			isOwner = reader.ReadBool(),
			sceneId = reader.ReadULong(),
			assetId = reader.ReadUInt(),
			position = reader.ReadVector3(),
			rotation = reader.ReadQuaternion(),
			scale = reader.ReadVector3(),
			payload = reader.ReadBytesAndSizeSegment()
		};
	}

	public static void _Write_Mirror_002ESpawnMessage(NetworkWriter writer, SpawnMessage value)
	{
		writer.WriteUInt(value.netId);
		writer.WriteBool(value.isLocalPlayer);
		writer.WriteBool(value.isOwner);
		writer.WriteULong(value.sceneId);
		writer.WriteUInt(value.assetId);
		writer.WriteVector3(value.position);
		writer.WriteQuaternion(value.rotation);
		writer.WriteVector3(value.scale);
		writer.WriteBytesAndSizeSegment(value.payload);
	}

	public static ChangeOwnerMessage _Read_Mirror_002EChangeOwnerMessage(NetworkReader reader)
	{
		return new ChangeOwnerMessage
		{
			netId = reader.ReadUInt(),
			isOwner = reader.ReadBool(),
			isLocalPlayer = reader.ReadBool()
		};
	}

	public static void _Write_Mirror_002EChangeOwnerMessage(NetworkWriter writer, ChangeOwnerMessage value)
	{
		writer.WriteUInt(value.netId);
		writer.WriteBool(value.isOwner);
		writer.WriteBool(value.isLocalPlayer);
	}

	public static ObjectSpawnStartedMessage _Read_Mirror_002EObjectSpawnStartedMessage(NetworkReader reader)
	{
		return default(ObjectSpawnStartedMessage);
	}

	public static void _Write_Mirror_002EObjectSpawnStartedMessage(NetworkWriter writer, ObjectSpawnStartedMessage value)
	{
	}

	public static ObjectSpawnFinishedMessage _Read_Mirror_002EObjectSpawnFinishedMessage(NetworkReader reader)
	{
		return default(ObjectSpawnFinishedMessage);
	}

	public static void _Write_Mirror_002EObjectSpawnFinishedMessage(NetworkWriter writer, ObjectSpawnFinishedMessage value)
	{
	}

	public static ObjectDestroyMessage _Read_Mirror_002EObjectDestroyMessage(NetworkReader reader)
	{
		return new ObjectDestroyMessage
		{
			netId = reader.ReadUInt()
		};
	}

	public static void _Write_Mirror_002EObjectDestroyMessage(NetworkWriter writer, ObjectDestroyMessage value)
	{
		writer.WriteUInt(value.netId);
	}

	public static ObjectHideMessage _Read_Mirror_002EObjectHideMessage(NetworkReader reader)
	{
		return new ObjectHideMessage
		{
			netId = reader.ReadUInt()
		};
	}

	public static void _Write_Mirror_002EObjectHideMessage(NetworkWriter writer, ObjectHideMessage value)
	{
		writer.WriteUInt(value.netId);
	}

	public static EntityStateMessage _Read_Mirror_002EEntityStateMessage(NetworkReader reader)
	{
		return new EntityStateMessage
		{
			netId = reader.ReadUInt(),
			payload = reader.ReadBytesAndSizeSegment()
		};
	}

	public static void _Write_Mirror_002EEntityStateMessage(NetworkWriter writer, EntityStateMessage value)
	{
		writer.WriteUInt(value.netId);
		writer.WriteBytesAndSizeSegment(value.payload);
	}

	public static NetworkPingMessage _Read_Mirror_002ENetworkPingMessage(NetworkReader reader)
	{
		return new NetworkPingMessage
		{
			clientTime = reader.ReadDouble()
		};
	}

	public static void _Write_Mirror_002ENetworkPingMessage(NetworkWriter writer, NetworkPingMessage value)
	{
		writer.WriteDouble(value.clientTime);
	}

	public static NetworkPongMessage _Read_Mirror_002ENetworkPongMessage(NetworkReader reader)
	{
		return new NetworkPongMessage
		{
			clientTime = reader.ReadDouble()
		};
	}

	public static void _Write_Mirror_002ENetworkPongMessage(NetworkWriter writer, NetworkPongMessage value)
	{
		writer.WriteDouble(value.clientTime);
	}

	public static mpCalls.c_GameSpeed _Read_mpCalls_002Fc_GameSpeed(NetworkReader reader)
	{
		return new mpCalls.c_GameSpeed
		{
			speed = reader.ReadInt()
		};
	}

	public static void _Write_mpCalls_002Fc_GameSpeed(NetworkWriter writer, mpCalls.c_GameSpeed value)
	{
		writer.WriteInt(value.speed);
	}

	public static mpCalls.c_NpcGameName _Read_mpCalls_002Fc_NpcGameName(NetworkReader reader)
	{
		return new mpCalls.c_NpcGameName
		{
			slot = reader.ReadInt(),
			ip = reader.ReadBool()
		};
	}

	public static void _Write_mpCalls_002Fc_NpcGameName(NetworkWriter writer, mpCalls.c_NpcGameName value)
	{
		writer.WriteInt(value.slot);
		writer.WriteBool(value.ip);
	}

	public static mpCalls.c_BlockContractGame _Read_mpCalls_002Fc_BlockContractGame(NetworkReader reader)
	{
		return new mpCalls.c_BlockContractGame
		{
			myID = reader.ReadInt(),
			block = reader.ReadBool()
		};
	}

	public static void _Write_mpCalls_002Fc_BlockContractGame(NetworkWriter writer, mpCalls.c_BlockContractGame value)
	{
		writer.WriteInt(value.myID);
		writer.WriteBool(value.block);
	}

	public static mpCalls.c_Publisher _Read_mpCalls_002Fc_Publisher(NetworkReader reader)
	{
		return new mpCalls.c_Publisher
		{
			myID = reader.ReadInt(),
			isUnlocked = reader.ReadBool(),
			name_EN = reader.ReadString(),
			name_GE = reader.ReadString(),
			name_TU = reader.ReadString(),
			name_CH = reader.ReadString(),
			name_FR = reader.ReadString(),
			name_JA = reader.ReadString(),
			name_UA = reader.ReadString(),
			name_TH = reader.ReadString(),
			date_year = reader.ReadInt(),
			date_month = reader.ReadInt(),
			stars = reader.ReadFloat(),
			logoID = reader.ReadInt(),
			developer = reader.ReadBool(),
			publisher = reader.ReadBool(),
			onlyMobile = reader.ReadBool(),
			share = reader.ReadFloat(),
			fanGenre = reader.ReadInt(),
			firmenwert = reader.ReadLong(),
			notForSale = reader.ReadBool(),
			lockToBuy = reader.ReadInt(),
			isPlayer = reader.ReadBool(),
			ownerID = reader.ReadInt(),
			country = reader.ReadInt(),
			ownPlatform = reader.ReadBool(),
			exklusive = reader.ReadBool(),
			awards = _Read_System_002EInt32_005B_005D(reader)
		};
	}

	public static int[] _Read_System_002EInt32_005B_005D(NetworkReader reader)
	{
		return reader.ReadArray<int>();
	}

	public static void _Write_mpCalls_002Fc_Publisher(NetworkWriter writer, mpCalls.c_Publisher value)
	{
		writer.WriteInt(value.myID);
		writer.WriteBool(value.isUnlocked);
		writer.WriteString(value.name_EN);
		writer.WriteString(value.name_GE);
		writer.WriteString(value.name_TU);
		writer.WriteString(value.name_CH);
		writer.WriteString(value.name_FR);
		writer.WriteString(value.name_JA);
		writer.WriteString(value.name_UA);
		writer.WriteString(value.name_TH);
		writer.WriteInt(value.date_year);
		writer.WriteInt(value.date_month);
		writer.WriteFloat(value.stars);
		writer.WriteInt(value.logoID);
		writer.WriteBool(value.developer);
		writer.WriteBool(value.publisher);
		writer.WriteBool(value.onlyMobile);
		writer.WriteFloat(value.share);
		writer.WriteInt(value.fanGenre);
		writer.WriteLong(value.firmenwert);
		writer.WriteBool(value.notForSale);
		writer.WriteInt(value.lockToBuy);
		writer.WriteBool(value.isPlayer);
		writer.WriteInt(value.ownerID);
		writer.WriteInt(value.country);
		writer.WriteBool(value.ownPlatform);
		writer.WriteBool(value.exklusive);
		_Write_System_002EInt32_005B_005D(writer, value.awards);
	}

	public static void _Write_System_002EInt32_005B_005D(NetworkWriter writer, int[] value)
	{
		writer.WriteArray(value);
	}

	public static mpCalls.c_PublisherOwner _Read_mpCalls_002Fc_PublisherOwner(NetworkReader reader)
	{
		return new mpCalls.c_PublisherOwner
		{
			myID = reader.ReadInt(),
			ownerID = reader.ReadInt()
		};
	}

	public static void _Write_mpCalls_002Fc_PublisherOwner(NetworkWriter writer, mpCalls.c_PublisherOwner value)
	{
		writer.WriteInt(value.myID);
		writer.WriteInt(value.ownerID);
	}

	public static mpCalls.c_PublisherTochterfirmaSettings _Read_mpCalls_002Fc_PublisherTochterfirmaSettings(NetworkReader reader)
	{
		return new mpCalls.c_PublisherTochterfirmaSettings
		{
			myID = reader.ReadInt(),
			tf_geschlossen = reader.ReadBool(),
			tf_autoRelease = reader.ReadBool(),
			tf_onlyPlayerConsole = reader.ReadBool(),
			tf_allowMMO = reader.ReadBool(),
			tf_allowF2P = reader.ReadBool(),
			tf_allowAddon = reader.ReadBool(),
			tf_noArcade = reader.ReadBool(),
			tf_noHandy = reader.ReadBool(),
			tf_noRetro = reader.ReadBool(),
			tf_noPorts = reader.ReadBool(),
			tf_noBudget = reader.ReadBool(),
			tf_noGOTY = reader.ReadBool(),
			tf_noBundles = reader.ReadBool(),
			tf_noAddonBundles = reader.ReadBool(),
			tf_noRemaster = reader.ReadBool(),
			tf_noSpinoffs = reader.ReadBool(),
			tf_autoGamePass = reader.ReadBool(),
			tf_ownPublisher = reader.ReadBool(),
			tf_publisher = reader.ReadBool(),
			tf_developer = reader.ReadBool(),
			tf_entwicklungsdauer = reader.ReadInt(),
			tf_gameSize = reader.ReadInt(),
			tf_gameGenre = reader.ReadInt(),
			tf_gameTopic = reader.ReadInt(),
			tf_engine = reader.ReadInt(),
			tf_autoReleaseVal = reader.ReadInt(),
			tf_ownPublisherPriority = reader.ReadInt(),
			tf_ipFocus = _Read_System_002EInt32_005B_005D(reader),
			tf_platformFocus = _Read_System_002EInt32_005B_005D(reader)
		};
	}

	public static void _Write_mpCalls_002Fc_PublisherTochterfirmaSettings(NetworkWriter writer, mpCalls.c_PublisherTochterfirmaSettings value)
	{
		writer.WriteInt(value.myID);
		writer.WriteBool(value.tf_geschlossen);
		writer.WriteBool(value.tf_autoRelease);
		writer.WriteBool(value.tf_onlyPlayerConsole);
		writer.WriteBool(value.tf_allowMMO);
		writer.WriteBool(value.tf_allowF2P);
		writer.WriteBool(value.tf_allowAddon);
		writer.WriteBool(value.tf_noArcade);
		writer.WriteBool(value.tf_noHandy);
		writer.WriteBool(value.tf_noRetro);
		writer.WriteBool(value.tf_noPorts);
		writer.WriteBool(value.tf_noBudget);
		writer.WriteBool(value.tf_noGOTY);
		writer.WriteBool(value.tf_noBundles);
		writer.WriteBool(value.tf_noAddonBundles);
		writer.WriteBool(value.tf_noRemaster);
		writer.WriteBool(value.tf_noSpinoffs);
		writer.WriteBool(value.tf_autoGamePass);
		writer.WriteBool(value.tf_ownPublisher);
		writer.WriteBool(value.tf_publisher);
		writer.WriteBool(value.tf_developer);
		writer.WriteInt(value.tf_entwicklungsdauer);
		writer.WriteInt(value.tf_gameSize);
		writer.WriteInt(value.tf_gameGenre);
		writer.WriteInt(value.tf_gameTopic);
		writer.WriteInt(value.tf_engine);
		writer.WriteInt(value.tf_autoReleaseVal);
		writer.WriteInt(value.tf_ownPublisherPriority);
		_Write_System_002EInt32_005B_005D(writer, value.tf_ipFocus);
		_Write_System_002EInt32_005B_005D(writer, value.tf_platformFocus);
	}

	public static mpCalls.c_Forschung _Read_mpCalls_002Fc_Forschung(NetworkReader reader)
	{
		return new mpCalls.c_Forschung
		{
			playerID = reader.ReadInt(),
			forschungSonstiges = _Read_System_002EBoolean_005B_005D(reader),
			genres = _Read_System_002EBoolean_005B_005D(reader),
			themes = _Read_System_002EBoolean_005B_005D(reader),
			engineFeatures = _Read_System_002EBoolean_005B_005D(reader),
			gameplayFeatures = _Read_System_002EBoolean_005B_005D(reader),
			hardware = _Read_System_002EBoolean_005B_005D(reader),
			hardwareFeatures = _Read_System_002EBoolean_005B_005D(reader)
		};
	}

	public static bool[] _Read_System_002EBoolean_005B_005D(NetworkReader reader)
	{
		return reader.ReadArray<bool>();
	}

	public static void _Write_mpCalls_002Fc_Forschung(NetworkWriter writer, mpCalls.c_Forschung value)
	{
		writer.WriteInt(value.playerID);
		_Write_System_002EBoolean_005B_005D(writer, value.forschungSonstiges);
		_Write_System_002EBoolean_005B_005D(writer, value.genres);
		_Write_System_002EBoolean_005B_005D(writer, value.themes);
		_Write_System_002EBoolean_005B_005D(writer, value.engineFeatures);
		_Write_System_002EBoolean_005B_005D(writer, value.gameplayFeatures);
		_Write_System_002EBoolean_005B_005D(writer, value.hardware);
		_Write_System_002EBoolean_005B_005D(writer, value.hardwareFeatures);
	}

	public static void _Write_System_002EBoolean_005B_005D(NetworkWriter writer, bool[] value)
	{
		writer.WriteArray(value);
	}

	public static mpCalls.c_Help _Read_mpCalls_002Fc_Help(NetworkReader reader)
	{
		return new mpCalls.c_Help
		{
			playerID = reader.ReadInt(),
			toPlayerID = reader.ReadInt(),
			what = reader.ReadInt(),
			valueA = reader.ReadInt(),
			valueB = reader.ReadInt(),
			valueC = reader.ReadInt()
		};
	}

	public static void _Write_mpCalls_002Fc_Help(NetworkWriter writer, mpCalls.c_Help value)
	{
		writer.WriteInt(value.playerID);
		writer.WriteInt(value.toPlayerID);
		writer.WriteInt(value.what);
		writer.WriteInt(value.valueA);
		writer.WriteInt(value.valueB);
		writer.WriteInt(value.valueC);
	}

	public static mpCalls.c_ObjectDelete _Read_mpCalls_002Fc_ObjectDelete(NetworkReader reader)
	{
		return new mpCalls.c_ObjectDelete
		{
			playerID = reader.ReadInt(),
			objectID = reader.ReadInt()
		};
	}

	public static void _Write_mpCalls_002Fc_ObjectDelete(NetworkWriter writer, mpCalls.c_ObjectDelete value)
	{
		writer.WriteInt(value.playerID);
		writer.WriteInt(value.objectID);
	}

	public static mpCalls.c_Object _Read_mpCalls_002Fc_Object(NetworkReader reader)
	{
		return new mpCalls.c_Object
		{
			playerID = reader.ReadInt(),
			objectID = reader.ReadInt(),
			typ = reader.ReadInt(),
			x = reader.ReadFloat(),
			y = reader.ReadFloat(),
			rot = reader.ReadFloat()
		};
	}

	public static void _Write_mpCalls_002Fc_Object(NetworkWriter writer, mpCalls.c_Object value)
	{
		writer.WriteInt(value.playerID);
		writer.WriteInt(value.objectID);
		writer.WriteInt(value.typ);
		writer.WriteFloat(value.x);
		writer.WriteFloat(value.y);
		writer.WriteFloat(value.rot);
	}

	public static mpCalls.c_Map _Read_mpCalls_002Fc_Map(NetworkReader reader)
	{
		return new mpCalls.c_Map
		{
			playerID = reader.ReadInt(),
			x = NetworkReaderExtensions.ReadByte(reader),
			y = NetworkReaderExtensions.ReadByte(reader),
			id = reader.ReadInt(),
			typ = reader.ReadInt(),
			door = reader.ReadInt(),
			window = reader.ReadInt()
		};
	}

	public static void _Write_mpCalls_002Fc_Map(NetworkWriter writer, mpCalls.c_Map value)
	{
		writer.WriteInt(value.playerID);
		NetworkWriterExtensions.WriteByte(writer, value.x);
		NetworkWriterExtensions.WriteByte(writer, value.y);
		writer.WriteInt(value.id);
		writer.WriteInt(value.typ);
		writer.WriteInt(value.door);
		writer.WriteInt(value.window);
	}

	public static mpCalls.c_Trend _Read_mpCalls_002Fc_Trend(NetworkReader reader)
	{
		return new mpCalls.c_Trend
		{
			trendWeeks = reader.ReadInt(),
			trendTheme = reader.ReadInt(),
			trendAntiTheme = reader.ReadInt(),
			trendGenre = reader.ReadInt(),
			trendAntiGenre = reader.ReadInt()
		};
	}

	public static void _Write_mpCalls_002Fc_Trend(NetworkWriter writer, mpCalls.c_Trend value)
	{
		writer.WriteInt(value.trendWeeks);
		writer.WriteInt(value.trendTheme);
		writer.WriteInt(value.trendAntiTheme);
		writer.WriteInt(value.trendGenre);
		writer.WriteInt(value.trendAntiGenre);
	}

	public static mpCalls.c_Payment _Read_mpCalls_002Fc_Payment(NetworkReader reader)
	{
		return new mpCalls.c_Payment
		{
			playerID = reader.ReadInt(),
			toPlayerID = reader.ReadInt(),
			what = reader.ReadInt(),
			money = reader.ReadInt(),
			objectID = reader.ReadInt()
		};
	}

	public static void _Write_mpCalls_002Fc_Payment(NetworkWriter writer, mpCalls.c_Payment value)
	{
		writer.WriteInt(value.playerID);
		writer.WriteInt(value.toPlayerID);
		writer.WriteInt(value.what);
		writer.WriteInt(value.money);
		writer.WriteInt(value.objectID);
	}

	public static mpCalls.c_Engine _Read_mpCalls_002Fc_Engine(NetworkReader reader)
	{
		return new mpCalls.c_Engine
		{
			myID = reader.ReadInt(),
			ownerID = reader.ReadInt(),
			isUnlocked = reader.ReadBool(),
			gekauft = reader.ReadBool(),
			myName = reader.ReadString(),
			features = _Read_System_002EBoolean_005B_005D(reader),
			spezialgenre = reader.ReadInt(),
			spezialplatform = reader.ReadInt(),
			sellEngine = reader.ReadBool(),
			preis = reader.ReadInt(),
			gewinnbeteiligung = reader.ReadInt(),
			marktdominanz = reader.ReadFloat()
		};
	}

	public static void _Write_mpCalls_002Fc_Engine(NetworkWriter writer, mpCalls.c_Engine value)
	{
		writer.WriteInt(value.myID);
		writer.WriteInt(value.ownerID);
		writer.WriteBool(value.isUnlocked);
		writer.WriteBool(value.gekauft);
		writer.WriteString(value.myName);
		_Write_System_002EBoolean_005B_005D(writer, value.features);
		writer.WriteInt(value.spezialgenre);
		writer.WriteInt(value.spezialplatform);
		writer.WriteBool(value.sellEngine);
		writer.WriteInt(value.preis);
		writer.WriteInt(value.gewinnbeteiligung);
		writer.WriteFloat(value.marktdominanz);
	}

	public static mpCalls.c_EnginePublisherBuyed _Read_mpCalls_002Fc_EnginePublisherBuyed(NetworkReader reader)
	{
		return new mpCalls.c_EnginePublisherBuyed
		{
			myID = reader.ReadInt(),
			publisherBuyed = _Read_System_002EBoolean_005B_005D(reader)
		};
	}

	public static void _Write_mpCalls_002Fc_EnginePublisherBuyed(NetworkWriter writer, mpCalls.c_EnginePublisherBuyed value)
	{
		writer.WriteInt(value.myID);
		_Write_System_002EBoolean_005B_005D(writer, value.publisherBuyed);
	}

	public static mpCalls.c_Platform _Read_mpCalls_002Fc_Platform(NetworkReader reader)
	{
		return new mpCalls.c_Platform
		{
			myID = reader.ReadInt(),
			date_year = reader.ReadInt(),
			date_month = reader.ReadInt(),
			date_year_end = reader.ReadInt(),
			date_month_end = reader.ReadInt(),
			price = reader.ReadInt(),
			dev_costs = reader.ReadInt(),
			tech = reader.ReadInt(),
			typ = reader.ReadInt(),
			marktanteil = reader.ReadFloat(),
			needFeatures = _Read_System_002EInt32_005B_005D(reader),
			units = reader.ReadInt(),
			units_max = reader.ReadInt(),
			minGamePassGames = reader.ReadInt(),
			name_EN = reader.ReadString(),
			name_GE = reader.ReadString(),
			name_TU = reader.ReadString(),
			name_CH = reader.ReadString(),
			name_FR = reader.ReadString(),
			name_HU = reader.ReadString(),
			name_JA = reader.ReadString(),
			name_PL = reader.ReadString(),
			name_UA = reader.ReadString(),
			name_TH = reader.ReadString(),
			manufacturer_EN = reader.ReadString(),
			manufacturer_GE = reader.ReadString(),
			manufacturer_TU = reader.ReadString(),
			manufacturer_CH = reader.ReadString(),
			manufacturer_FR = reader.ReadString(),
			manufacturer_HU = reader.ReadString(),
			manufacturer_JA = reader.ReadString(),
			manufacturer_PL = reader.ReadString(),
			manufacturer_UA = reader.ReadString(),
			manufacturer_TH = reader.ReadString(),
			pic1_file = reader.ReadString(),
			pic2_file = reader.ReadString(),
			pic2_year = reader.ReadInt(),
			games = reader.ReadInt(),
			exklusivGames = reader.ReadInt(),
			erfahrung = reader.ReadInt(),
			isUnlocked = reader.ReadBool(),
			inBesitz = reader.ReadBool(),
			vomMarktGenommen = reader.ReadBool(),
			complex = reader.ReadInt(),
			internet = reader.ReadBool(),
			powerFromMarket = reader.ReadFloat(),
			myName = reader.ReadString(),
			ownerID = reader.ReadInt(),
			gameID = reader.ReadInt(),
			anzController = reader.ReadInt(),
			consoleColor = reader.ReadVector3(),
			component_cpu = reader.ReadInt(),
			component_gfx = reader.ReadInt(),
			component_ram = reader.ReadInt(),
			component_hdd = reader.ReadInt(),
			component_sfx = reader.ReadInt(),
			component_cooling = reader.ReadInt(),
			component_disc = reader.ReadInt(),
			component_controller = reader.ReadInt(),
			component_case = reader.ReadInt(),
			component_monitor = reader.ReadInt(),
			hwFeatures = _Read_System_002EBoolean_005B_005D(reader),
			devPoints = reader.ReadFloat(),
			devPointsStart = reader.ReadFloat(),
			entwicklungsKosten = reader.ReadLong(),
			einnahmen = reader.ReadLong(),
			hype = reader.ReadFloat(),
			startProduktionskosten = reader.ReadInt(),
			verkaufspreis = reader.ReadInt(),
			kostenreduktion = reader.ReadFloat(),
			autoPreis = reader.ReadBool(),
			thridPartyGames = reader.ReadBool(),
			umsatzTotal = reader.ReadLong(),
			sellsPerWeek = _Read_System_002EInt32_005B_005D(reader),
			weeksOnMarket = reader.ReadInt(),
			review = reader.ReadFloat(),
			performancePoints = reader.ReadInt(),
			nachfolgerID = reader.ReadInt(),
			vorgaengerID = reader.ReadInt(),
			proVersion = reader.ReadBool(),
			proName = reader.ReadString(),
			subventionMoney = reader.ReadInt(),
			subventionGameSize = _Read_System_002EBoolean_005B_005D(reader)
		};
	}

	public static void _Write_mpCalls_002Fc_Platform(NetworkWriter writer, mpCalls.c_Platform value)
	{
		writer.WriteInt(value.myID);
		writer.WriteInt(value.date_year);
		writer.WriteInt(value.date_month);
		writer.WriteInt(value.date_year_end);
		writer.WriteInt(value.date_month_end);
		writer.WriteInt(value.price);
		writer.WriteInt(value.dev_costs);
		writer.WriteInt(value.tech);
		writer.WriteInt(value.typ);
		writer.WriteFloat(value.marktanteil);
		_Write_System_002EInt32_005B_005D(writer, value.needFeatures);
		writer.WriteInt(value.units);
		writer.WriteInt(value.units_max);
		writer.WriteInt(value.minGamePassGames);
		writer.WriteString(value.name_EN);
		writer.WriteString(value.name_GE);
		writer.WriteString(value.name_TU);
		writer.WriteString(value.name_CH);
		writer.WriteString(value.name_FR);
		writer.WriteString(value.name_HU);
		writer.WriteString(value.name_JA);
		writer.WriteString(value.name_PL);
		writer.WriteString(value.name_UA);
		writer.WriteString(value.name_TH);
		writer.WriteString(value.manufacturer_EN);
		writer.WriteString(value.manufacturer_GE);
		writer.WriteString(value.manufacturer_TU);
		writer.WriteString(value.manufacturer_CH);
		writer.WriteString(value.manufacturer_FR);
		writer.WriteString(value.manufacturer_HU);
		writer.WriteString(value.manufacturer_JA);
		writer.WriteString(value.manufacturer_PL);
		writer.WriteString(value.manufacturer_UA);
		writer.WriteString(value.manufacturer_TH);
		writer.WriteString(value.pic1_file);
		writer.WriteString(value.pic2_file);
		writer.WriteInt(value.pic2_year);
		writer.WriteInt(value.games);
		writer.WriteInt(value.exklusivGames);
		writer.WriteInt(value.erfahrung);
		writer.WriteBool(value.isUnlocked);
		writer.WriteBool(value.inBesitz);
		writer.WriteBool(value.vomMarktGenommen);
		writer.WriteInt(value.complex);
		writer.WriteBool(value.internet);
		writer.WriteFloat(value.powerFromMarket);
		writer.WriteString(value.myName);
		writer.WriteInt(value.ownerID);
		writer.WriteInt(value.gameID);
		writer.WriteInt(value.anzController);
		writer.WriteVector3(value.consoleColor);
		writer.WriteInt(value.component_cpu);
		writer.WriteInt(value.component_gfx);
		writer.WriteInt(value.component_ram);
		writer.WriteInt(value.component_hdd);
		writer.WriteInt(value.component_sfx);
		writer.WriteInt(value.component_cooling);
		writer.WriteInt(value.component_disc);
		writer.WriteInt(value.component_controller);
		writer.WriteInt(value.component_case);
		writer.WriteInt(value.component_monitor);
		_Write_System_002EBoolean_005B_005D(writer, value.hwFeatures);
		writer.WriteFloat(value.devPoints);
		writer.WriteFloat(value.devPointsStart);
		writer.WriteLong(value.entwicklungsKosten);
		writer.WriteLong(value.einnahmen);
		writer.WriteFloat(value.hype);
		writer.WriteInt(value.startProduktionskosten);
		writer.WriteInt(value.verkaufspreis);
		writer.WriteFloat(value.kostenreduktion);
		writer.WriteBool(value.autoPreis);
		writer.WriteBool(value.thridPartyGames);
		writer.WriteLong(value.umsatzTotal);
		_Write_System_002EInt32_005B_005D(writer, value.sellsPerWeek);
		writer.WriteInt(value.weeksOnMarket);
		writer.WriteFloat(value.review);
		writer.WriteInt(value.performancePoints);
		writer.WriteInt(value.nachfolgerID);
		writer.WriteInt(value.vorgaengerID);
		writer.WriteBool(value.proVersion);
		writer.WriteString(value.proName);
		writer.WriteInt(value.subventionMoney);
		_Write_System_002EBoolean_005B_005D(writer, value.subventionGameSize);
	}

	public static mpCalls.c_PlatformRemoveFromMarket _Read_mpCalls_002Fc_PlatformRemoveFromMarket(NetworkReader reader)
	{
		return new mpCalls.c_PlatformRemoveFromMarket
		{
			platformID = reader.ReadInt()
		};
	}

	public static void _Write_mpCalls_002Fc_PlatformRemoveFromMarket(NetworkWriter writer, mpCalls.c_PlatformRemoveFromMarket value)
	{
		writer.WriteInt(value.platformID);
	}

	public static mpCalls.c_Chat _Read_mpCalls_002Fc_Chat(NetworkReader reader)
	{
		return new mpCalls.c_Chat
		{
			playerID = reader.ReadInt(),
			text = reader.ReadString()
		};
	}

	public static void _Write_mpCalls_002Fc_Chat(NetworkWriter writer, mpCalls.c_Chat value)
	{
		writer.WriteInt(value.playerID);
		writer.WriteString(value.text);
	}

	public static mpCalls.c_Command _Read_mpCalls_002Fc_Command(NetworkReader reader)
	{
		return new mpCalls.c_Command
		{
			playerID = reader.ReadInt(),
			command = reader.ReadInt()
		};
	}

	public static void _Write_mpCalls_002Fc_Command(NetworkWriter writer, mpCalls.c_Command value)
	{
		writer.WriteInt(value.playerID);
		writer.WriteInt(value.command);
	}

	public static mpCalls.c_Money _Read_mpCalls_002Fc_Money(NetworkReader reader)
	{
		return new mpCalls.c_Money
		{
			playerID = reader.ReadInt(),
			money = reader.ReadLong(),
			fans = reader.ReadInt()
		};
	}

	public static void _Write_mpCalls_002Fc_Money(NetworkWriter writer, mpCalls.c_Money value)
	{
		writer.WriteInt(value.playerID);
		writer.WriteLong(value.money);
		writer.WriteInt(value.fans);
	}

	public static mpCalls.c_PlayerInfos _Read_mpCalls_002Fc_PlayerInfos(NetworkReader reader)
	{
		return new mpCalls.c_PlayerInfos
		{
			playerID = reader.ReadInt(),
			playerName = reader.ReadString(),
			ready = reader.ReadBool()
		};
	}

	public static void _Write_mpCalls_002Fc_PlayerInfos(NetworkWriter writer, mpCalls.c_PlayerInfos value)
	{
		writer.WriteInt(value.playerID);
		writer.WriteString(value.playerName);
		writer.WriteBool(value.ready);
	}

	public static mpCalls.c_DeleteArbeitsmarkt _Read_mpCalls_002Fc_DeleteArbeitsmarkt(NetworkReader reader)
	{
		return new mpCalls.c_DeleteArbeitsmarkt
		{
			playerID = reader.ReadInt(),
			objectID = reader.ReadInt(),
			eingestellt = reader.ReadBool()
		};
	}

	public static void _Write_mpCalls_002Fc_DeleteArbeitsmarkt(NetworkWriter writer, mpCalls.c_DeleteArbeitsmarkt value)
	{
		writer.WriteInt(value.playerID);
		writer.WriteInt(value.objectID);
		writer.WriteBool(value.eingestellt);
	}

	public static mpCalls.c_BuyLizenz _Read_mpCalls_002Fc_BuyLizenz(NetworkReader reader)
	{
		return new mpCalls.c_BuyLizenz
		{
			playerID = reader.ReadInt(),
			objectID = reader.ReadInt()
		};
	}

	public static void _Write_mpCalls_002Fc_BuyLizenz(NetworkWriter writer, mpCalls.c_BuyLizenz value)
	{
		writer.WriteInt(value.playerID);
		writer.WriteInt(value.objectID);
	}

	public static mpCalls.c_exklusivKonsolenSells _Read_mpCalls_002Fc_exklusivKonsolenSells(NetworkReader reader)
	{
		return new mpCalls.c_exklusivKonsolenSells
		{
			gameID = reader.ReadInt(),
			exklusivKonsolenSells = reader.ReadLong()
		};
	}

	public static void _Write_mpCalls_002Fc_exklusivKonsolenSells(NetworkWriter writer, mpCalls.c_exklusivKonsolenSells value)
	{
		writer.WriteInt(value.gameID);
		writer.WriteLong(value.exklusivKonsolenSells);
	}

	public static mpCalls.c_GameDestroy _Read_mpCalls_002Fc_GameDestroy(NetworkReader reader)
	{
		return new mpCalls.c_GameDestroy
		{
			gameID = reader.ReadInt()
		};
	}

	public static void _Write_mpCalls_002Fc_GameDestroy(NetworkWriter writer, mpCalls.c_GameDestroy value)
	{
		writer.WriteInt(value.gameID);
	}

	public static mpCalls.c_GameRemoveFromMarket _Read_mpCalls_002Fc_GameRemoveFromMarket(NetworkReader reader)
	{
		return new mpCalls.c_GameRemoveFromMarket
		{
			gameID = reader.ReadInt()
		};
	}

	public static void _Write_mpCalls_002Fc_GameRemoveFromMarket(NetworkWriter writer, mpCalls.c_GameRemoveFromMarket value)
	{
		writer.WriteInt(value.gameID);
	}

	public static mpCalls.c_GameOwner _Read_mpCalls_002Fc_GameOwner(NetworkReader reader)
	{
		return new mpCalls.c_GameOwner
		{
			gameID = reader.ReadInt(),
			ownerID = reader.ReadInt()
		};
	}

	public static void _Write_mpCalls_002Fc_GameOwner(NetworkWriter writer, mpCalls.c_GameOwner value)
	{
		writer.WriteInt(value.gameID);
		writer.WriteInt(value.ownerID);
	}

	public static mpCalls.c_GameIpSell _Read_mpCalls_002Fc_GameIpSell(NetworkReader reader)
	{
		return new mpCalls.c_GameIpSell
		{
			gameID = reader.ReadInt(),
			ipToSell = reader.ReadBool()
		};
	}

	public static void _Write_mpCalls_002Fc_GameIpSell(NetworkWriter writer, mpCalls.c_GameIpSell value)
	{
		writer.WriteInt(value.gameID);
		writer.WriteBool(value.ipToSell);
	}

	public static mpCalls.c_GameIpPoints _Read_mpCalls_002Fc_GameIpPoints(NetworkReader reader)
	{
		return new mpCalls.c_GameIpPoints
		{
			gameID = reader.ReadInt(),
			ipPunkte = reader.ReadFloat(),
			ipTime = reader.ReadInt()
		};
	}

	public static void _Write_mpCalls_002Fc_GameIpPoints(NetworkWriter writer, mpCalls.c_GameIpPoints value)
	{
		writer.WriteInt(value.gameID);
		writer.WriteFloat(value.ipPunkte);
		writer.WriteInt(value.ipTime);
	}

	public static mpCalls.c_GameSell _Read_mpCalls_002Fc_GameSell(NetworkReader reader)
	{
		return new mpCalls.c_GameSell
		{
			gameID = reader.ReadInt(),
			isOnMarket = reader.ReadBool(),
			weeksOnMarket = reader.ReadInt(),
			sellsTotal = reader.ReadLong(),
			sellsTotalStandard = reader.ReadLong(),
			sellsTotalDeluxe = reader.ReadLong(),
			sellsTotalCollectors = reader.ReadLong(),
			sellsTotalOnline = reader.ReadLong(),
			abonnements = reader.ReadLong(),
			abonnementsWoche = reader.ReadLong(),
			bestAbonnements = reader.ReadLong(),
			exklusivKonsolenSells = reader.ReadLong(),
			userPositiv = reader.ReadInt(),
			userNegativ = reader.ReadInt(),
			costs_entwicklung = reader.ReadLong(),
			costs_mitarbeiter = reader.ReadLong(),
			costs_marketing = reader.ReadLong(),
			costs_enginegebuehren = reader.ReadLong(),
			costs_server = reader.ReadLong(),
			costs_production = reader.ReadLong(),
			costs_updates = reader.ReadLong(),
			points_gameplay = reader.ReadFloat(),
			points_grafik = reader.ReadFloat(),
			points_sound = reader.ReadFloat(),
			points_technik = reader.ReadFloat(),
			points_bugs = reader.ReadFloat(),
			points_bugsInvis = reader.ReadFloat(),
			umsatzTotal = reader.ReadLong(),
			umsatzInApp = reader.ReadLong(),
			umsatzAbos = reader.ReadLong(),
			bestChartPosition = reader.ReadInt(),
			lastChartPosition = reader.ReadInt(),
			f2pInteresse = reader.ReadFloat(),
			mmoInteresse = reader.ReadFloat(),
			vorbestellungen = reader.ReadInt(),
			realsticPower = reader.ReadFloat(),
			hype = reader.ReadFloat(),
			stornierungen = reader.ReadInt(),
			commercialFlop = reader.ReadBool(),
			commercialHit = reader.ReadBool(),
			freigabeBudget = reader.ReadInt(),
			releaseDate = reader.ReadInt(),
			inAppPurchaseWeek = reader.ReadInt(),
			sellsPerWeek = _Read_System_002EInt32_005B_005D(reader),
			verkaufspreis = _Read_System_002EInt32_005B_005D(reader)
		};
	}

	public static void _Write_mpCalls_002Fc_GameSell(NetworkWriter writer, mpCalls.c_GameSell value)
	{
		writer.WriteInt(value.gameID);
		writer.WriteBool(value.isOnMarket);
		writer.WriteInt(value.weeksOnMarket);
		writer.WriteLong(value.sellsTotal);
		writer.WriteLong(value.sellsTotalStandard);
		writer.WriteLong(value.sellsTotalDeluxe);
		writer.WriteLong(value.sellsTotalCollectors);
		writer.WriteLong(value.sellsTotalOnline);
		writer.WriteLong(value.abonnements);
		writer.WriteLong(value.abonnementsWoche);
		writer.WriteLong(value.bestAbonnements);
		writer.WriteLong(value.exklusivKonsolenSells);
		writer.WriteInt(value.userPositiv);
		writer.WriteInt(value.userNegativ);
		writer.WriteLong(value.costs_entwicklung);
		writer.WriteLong(value.costs_mitarbeiter);
		writer.WriteLong(value.costs_marketing);
		writer.WriteLong(value.costs_enginegebuehren);
		writer.WriteLong(value.costs_server);
		writer.WriteLong(value.costs_production);
		writer.WriteLong(value.costs_updates);
		writer.WriteFloat(value.points_gameplay);
		writer.WriteFloat(value.points_grafik);
		writer.WriteFloat(value.points_sound);
		writer.WriteFloat(value.points_technik);
		writer.WriteFloat(value.points_bugs);
		writer.WriteFloat(value.points_bugsInvis);
		writer.WriteLong(value.umsatzTotal);
		writer.WriteLong(value.umsatzInApp);
		writer.WriteLong(value.umsatzAbos);
		writer.WriteInt(value.bestChartPosition);
		writer.WriteInt(value.lastChartPosition);
		writer.WriteFloat(value.f2pInteresse);
		writer.WriteFloat(value.mmoInteresse);
		writer.WriteInt(value.vorbestellungen);
		writer.WriteFloat(value.realsticPower);
		writer.WriteFloat(value.hype);
		writer.WriteInt(value.stornierungen);
		writer.WriteBool(value.commercialFlop);
		writer.WriteBool(value.commercialHit);
		writer.WriteInt(value.freigabeBudget);
		writer.WriteInt(value.releaseDate);
		writer.WriteInt(value.inAppPurchaseWeek);
		_Write_System_002EInt32_005B_005D(writer, value.sellsPerWeek);
		_Write_System_002EInt32_005B_005D(writer, value.verkaufspreis);
	}

	public static mpCalls.c_Game _Read_mpCalls_002Fc_Game(NetworkReader reader)
	{
		return new mpCalls.c_Game
		{
			gameID = reader.ReadInt(),
			myName = reader.ReadString(),
			ipName = reader.ReadString(),
			playerGame = reader.ReadBool(),
			inDevelopment = reader.ReadBool(),
			developerID = reader.ReadInt(),
			publisherID = reader.ReadInt(),
			ownerID = reader.ReadInt(),
			engineID = reader.ReadInt(),
			hype = reader.ReadFloat(),
			isOnMarket = reader.ReadBool(),
			warBeiAwards = reader.ReadBool(),
			weeksOnMarket = reader.ReadInt(),
			usk = reader.ReadInt(),
			freigabeBudget = reader.ReadInt(),
			reviewGameplay = reader.ReadInt(),
			reviewGrafik = reader.ReadInt(),
			reviewSound = reader.ReadInt(),
			reviewSteuerung = reader.ReadInt(),
			reviewTotal = reader.ReadInt(),
			reviewGameplayText = reader.ReadInt(),
			reviewGrafikText = reader.ReadInt(),
			reviewSoundText = reader.ReadInt(),
			reviewSteuerungText = reader.ReadInt(),
			reviewTotalText = reader.ReadInt(),
			date_year = reader.ReadInt(),
			date_month = reader.ReadInt(),
			date_start_year = reader.ReadInt(),
			date_start_month = reader.ReadInt(),
			sellsTotal = reader.ReadLong(),
			umsatzTotal = reader.ReadLong(),
			costs_entwicklung = reader.ReadLong(),
			costs_mitarbeiter = reader.ReadLong(),
			costs_marketing = reader.ReadLong(),
			costs_enginegebuehren = reader.ReadLong(),
			costs_server = reader.ReadLong(),
			costs_production = reader.ReadLong(),
			costs_updates = reader.ReadLong(),
			typ_standard = reader.ReadBool(),
			typ_nachfolger = reader.ReadBool(),
			teile = reader.ReadInt(),
			typ_contractGame = reader.ReadBool(),
			typ_remaster = reader.ReadBool(),
			typ_spinoff = reader.ReadBool(),
			typ_addon = reader.ReadBool(),
			typ_addonStandalone = reader.ReadBool(),
			typ_mmoaddon = reader.ReadBool(),
			typ_bundle = reader.ReadBool(),
			typ_budget = reader.ReadBool(),
			typ_bundleAddon = reader.ReadBool(),
			typ_goty = reader.ReadBool(),
			originalGameID = reader.ReadInt(),
			portID = reader.ReadInt(),
			mainIP = reader.ReadInt(),
			ipPunkte = reader.ReadFloat(),
			exklusiv = reader.ReadBool(),
			herstellerExklusiv = reader.ReadBool(),
			retro = reader.ReadBool(),
			handy = reader.ReadBool(),
			arcade = reader.ReadBool(),
			goty = reader.ReadBool(),
			nachfolger_created = reader.ReadBool(),
			remaster_created = reader.ReadBool(),
			budget_created = reader.ReadBool(),
			goty_created = reader.ReadBool(),
			trendsetter = reader.ReadBool(),
			spielbericht = reader.ReadBool(),
			amountUpdates = reader.ReadInt(),
			bonusSellsUpdates = reader.ReadFloat(),
			amountAddons = reader.ReadInt(),
			bonusSellsAddons = reader.ReadFloat(),
			amountMMOAddons = reader.ReadInt(),
			bonusSellsMMOAddons = reader.ReadFloat(),
			addonQuality = reader.ReadFloat(),
			devAktFeature = reader.ReadInt(),
			devPoints = reader.ReadFloat(),
			devPointsStart = reader.ReadFloat(),
			devPoints_Gesamt = reader.ReadFloat(),
			devPointsStart_Gesamt = reader.ReadFloat(),
			points_gameplay = reader.ReadFloat(),
			points_grafik = reader.ReadFloat(),
			points_sound = reader.ReadFloat(),
			points_technik = reader.ReadFloat(),
			points_bugs = reader.ReadFloat(),
			beschreibung = reader.ReadString(),
			gameTyp = reader.ReadInt(),
			gameSize = reader.ReadInt(),
			gameZielgruppe = reader.ReadInt(),
			maingenre = reader.ReadInt(),
			subgenre = reader.ReadInt(),
			gameMainTheme = reader.ReadInt(),
			gameSubTheme = reader.ReadInt(),
			gameLicence = reader.ReadInt(),
			gameCopyProtect = reader.ReadInt(),
			gameAntiCheat = reader.ReadInt(),
			gameAP_Gameplay = reader.ReadInt(),
			gameAP_Grafik = reader.ReadInt(),
			gameAP_Sound = reader.ReadInt(),
			gameAP_Technik = reader.ReadInt(),
			gameLanguage = _Read_System_002EBoolean_005B_005D(reader),
			gameGameplayFeatures = _Read_System_002EBoolean_005B_005D(reader),
			gamePlatform = _Read_System_002EInt32_005B_005D(reader),
			gameEngineFeature = _Read_System_002EInt32_005B_005D(reader),
			gameplayFeatures_DevDone = _Read_System_002EBoolean_005B_005D(reader),
			engineFeature_DevDone = _Read_System_002EBoolean_005B_005D(reader),
			gameplayStudio = _Read_System_002EBoolean_005B_005D(reader),
			grafikStudio = _Read_System_002EBoolean_005B_005D(reader),
			soundStudio = _Read_System_002EBoolean_005B_005D(reader),
			motionCaptureStudio = _Read_System_002EBoolean_005B_005D(reader),
			bundleID = _Read_System_002EInt32_005B_005D(reader),
			portExist = _Read_System_002EBoolean_005B_005D(reader),
			sellsPerWeek = _Read_System_002EInt32_005B_005D(reader),
			verkaufspreis = _Read_System_002EInt32_005B_005D(reader),
			releaseDate = reader.ReadInt(),
			abonnements = reader.ReadLong(),
			abonnementsWoche = reader.ReadLong(),
			aboPreis = reader.ReadInt(),
			pubOffer = reader.ReadBool(),
			pubAngebot = reader.ReadBool(),
			pubAngebot_Weeks = reader.ReadInt(),
			pubAngebot_Verhandlung = reader.ReadFloat(),
			pubAngebot_Retail = reader.ReadBool(),
			pubAngebot_Digital = reader.ReadBool(),
			pubAngebot_Garantiesumme = reader.ReadInt(),
			pubAngebot_Gewinnbeteiligung = reader.ReadFloat(),
			auftragsspiel = reader.ReadBool(),
			auftragsspiel_gehalt = reader.ReadInt(),
			auftragsspiel_bonus = reader.ReadInt(),
			auftragsspiel_zeitInWochen = reader.ReadInt(),
			auftragsspiel_wochenAlsAngebot = reader.ReadInt(),
			auftragsspiel_zeitAbgelaufen = reader.ReadBool(),
			auftragsspiel_mindestbewertung = reader.ReadInt(),
			f2pConverted = reader.ReadBool(),
			angekuendigt = reader.ReadBool(),
			subvention = reader.ReadLong(),
			sonderIP = reader.ReadBool(),
			sonderIPMindestreview = reader.ReadInt(),
			myNameTeil1 = reader.ReadString(),
			engineGewinnbeteiligung = reader.ReadInt(),
			weeksInDevelopment = reader.ReadInt(),
			userPositiv = reader.ReadInt(),
			userNegativ = reader.ReadInt(),
			bestAbonnements = reader.ReadLong(),
			bestChartPosition = reader.ReadInt(),
			exklusivKonsolenSells = reader.ReadLong(),
			lastChartPosition = reader.ReadInt(),
			freeware = reader.ReadBool(),
			sellsTotalStandard = reader.ReadLong(),
			sellsTotalDeluxe = reader.ReadLong(),
			sellsTotalCollectors = reader.ReadLong(),
			sellsTotalOnline = reader.ReadLong(),
			points_bugsInvis = reader.ReadFloat(),
			umsatzInApp = reader.ReadLong(),
			umsatzAbos = reader.ReadLong(),
			f2pInteresse = reader.ReadFloat(),
			mmoInteresse = reader.ReadFloat(),
			vorbestellungen = reader.ReadInt(),
			realsticPower = reader.ReadFloat(),
			stornierungen = reader.ReadInt(),
			commercialFlop = reader.ReadBool(),
			commercialHit = reader.ReadBool(),
			inAppPurchaseWeek = reader.ReadInt(),
			arcadeCase = reader.ReadInt(),
			arcadeMonitor = reader.ReadInt(),
			arcadeJoystick = reader.ReadInt(),
			arcadeSound = reader.ReadInt(),
			arcadeProdCosts = reader.ReadInt(),
			finanzierung_Grundkosten = reader.ReadInt(),
			finanzierung_Technology = reader.ReadInt(),
			finanzierung_Kontent = reader.ReadInt(),
			retailVersion = reader.ReadBool(),
			digitalVersion = reader.ReadBool(),
			newGenreCombination = reader.ReadBool(),
			newTopicCombination = reader.ReadBool(),
			ipTime = reader.ReadInt(),
			npcLateinNumbers = reader.ReadBool(),
			mmoTOf2p_created = reader.ReadBool(),
			bundle_created = reader.ReadBool(),
			abosAddons = reader.ReadInt(),
			inAppPurchase = _Read_System_002EBoolean_005B_005D(reader),
			Designschwerpunkt = _Read_System_002EInt32_005B_005D(reader),
			Designausrichtung = _Read_System_002EInt32_005B_005D(reader)
		};
	}

	public static void _Write_mpCalls_002Fc_Game(NetworkWriter writer, mpCalls.c_Game value)
	{
		writer.WriteInt(value.gameID);
		writer.WriteString(value.myName);
		writer.WriteString(value.ipName);
		writer.WriteBool(value.playerGame);
		writer.WriteBool(value.inDevelopment);
		writer.WriteInt(value.developerID);
		writer.WriteInt(value.publisherID);
		writer.WriteInt(value.ownerID);
		writer.WriteInt(value.engineID);
		writer.WriteFloat(value.hype);
		writer.WriteBool(value.isOnMarket);
		writer.WriteBool(value.warBeiAwards);
		writer.WriteInt(value.weeksOnMarket);
		writer.WriteInt(value.usk);
		writer.WriteInt(value.freigabeBudget);
		writer.WriteInt(value.reviewGameplay);
		writer.WriteInt(value.reviewGrafik);
		writer.WriteInt(value.reviewSound);
		writer.WriteInt(value.reviewSteuerung);
		writer.WriteInt(value.reviewTotal);
		writer.WriteInt(value.reviewGameplayText);
		writer.WriteInt(value.reviewGrafikText);
		writer.WriteInt(value.reviewSoundText);
		writer.WriteInt(value.reviewSteuerungText);
		writer.WriteInt(value.reviewTotalText);
		writer.WriteInt(value.date_year);
		writer.WriteInt(value.date_month);
		writer.WriteInt(value.date_start_year);
		writer.WriteInt(value.date_start_month);
		writer.WriteLong(value.sellsTotal);
		writer.WriteLong(value.umsatzTotal);
		writer.WriteLong(value.costs_entwicklung);
		writer.WriteLong(value.costs_mitarbeiter);
		writer.WriteLong(value.costs_marketing);
		writer.WriteLong(value.costs_enginegebuehren);
		writer.WriteLong(value.costs_server);
		writer.WriteLong(value.costs_production);
		writer.WriteLong(value.costs_updates);
		writer.WriteBool(value.typ_standard);
		writer.WriteBool(value.typ_nachfolger);
		writer.WriteInt(value.teile);
		writer.WriteBool(value.typ_contractGame);
		writer.WriteBool(value.typ_remaster);
		writer.WriteBool(value.typ_spinoff);
		writer.WriteBool(value.typ_addon);
		writer.WriteBool(value.typ_addonStandalone);
		writer.WriteBool(value.typ_mmoaddon);
		writer.WriteBool(value.typ_bundle);
		writer.WriteBool(value.typ_budget);
		writer.WriteBool(value.typ_bundleAddon);
		writer.WriteBool(value.typ_goty);
		writer.WriteInt(value.originalGameID);
		writer.WriteInt(value.portID);
		writer.WriteInt(value.mainIP);
		writer.WriteFloat(value.ipPunkte);
		writer.WriteBool(value.exklusiv);
		writer.WriteBool(value.herstellerExklusiv);
		writer.WriteBool(value.retro);
		writer.WriteBool(value.handy);
		writer.WriteBool(value.arcade);
		writer.WriteBool(value.goty);
		writer.WriteBool(value.nachfolger_created);
		writer.WriteBool(value.remaster_created);
		writer.WriteBool(value.budget_created);
		writer.WriteBool(value.goty_created);
		writer.WriteBool(value.trendsetter);
		writer.WriteBool(value.spielbericht);
		writer.WriteInt(value.amountUpdates);
		writer.WriteFloat(value.bonusSellsUpdates);
		writer.WriteInt(value.amountAddons);
		writer.WriteFloat(value.bonusSellsAddons);
		writer.WriteInt(value.amountMMOAddons);
		writer.WriteFloat(value.bonusSellsMMOAddons);
		writer.WriteFloat(value.addonQuality);
		writer.WriteInt(value.devAktFeature);
		writer.WriteFloat(value.devPoints);
		writer.WriteFloat(value.devPointsStart);
		writer.WriteFloat(value.devPoints_Gesamt);
		writer.WriteFloat(value.devPointsStart_Gesamt);
		writer.WriteFloat(value.points_gameplay);
		writer.WriteFloat(value.points_grafik);
		writer.WriteFloat(value.points_sound);
		writer.WriteFloat(value.points_technik);
		writer.WriteFloat(value.points_bugs);
		writer.WriteString(value.beschreibung);
		writer.WriteInt(value.gameTyp);
		writer.WriteInt(value.gameSize);
		writer.WriteInt(value.gameZielgruppe);
		writer.WriteInt(value.maingenre);
		writer.WriteInt(value.subgenre);
		writer.WriteInt(value.gameMainTheme);
		writer.WriteInt(value.gameSubTheme);
		writer.WriteInt(value.gameLicence);
		writer.WriteInt(value.gameCopyProtect);
		writer.WriteInt(value.gameAntiCheat);
		writer.WriteInt(value.gameAP_Gameplay);
		writer.WriteInt(value.gameAP_Grafik);
		writer.WriteInt(value.gameAP_Sound);
		writer.WriteInt(value.gameAP_Technik);
		_Write_System_002EBoolean_005B_005D(writer, value.gameLanguage);
		_Write_System_002EBoolean_005B_005D(writer, value.gameGameplayFeatures);
		_Write_System_002EInt32_005B_005D(writer, value.gamePlatform);
		_Write_System_002EInt32_005B_005D(writer, value.gameEngineFeature);
		_Write_System_002EBoolean_005B_005D(writer, value.gameplayFeatures_DevDone);
		_Write_System_002EBoolean_005B_005D(writer, value.engineFeature_DevDone);
		_Write_System_002EBoolean_005B_005D(writer, value.gameplayStudio);
		_Write_System_002EBoolean_005B_005D(writer, value.grafikStudio);
		_Write_System_002EBoolean_005B_005D(writer, value.soundStudio);
		_Write_System_002EBoolean_005B_005D(writer, value.motionCaptureStudio);
		_Write_System_002EInt32_005B_005D(writer, value.bundleID);
		_Write_System_002EBoolean_005B_005D(writer, value.portExist);
		_Write_System_002EInt32_005B_005D(writer, value.sellsPerWeek);
		_Write_System_002EInt32_005B_005D(writer, value.verkaufspreis);
		writer.WriteInt(value.releaseDate);
		writer.WriteLong(value.abonnements);
		writer.WriteLong(value.abonnementsWoche);
		writer.WriteInt(value.aboPreis);
		writer.WriteBool(value.pubOffer);
		writer.WriteBool(value.pubAngebot);
		writer.WriteInt(value.pubAngebot_Weeks);
		writer.WriteFloat(value.pubAngebot_Verhandlung);
		writer.WriteBool(value.pubAngebot_Retail);
		writer.WriteBool(value.pubAngebot_Digital);
		writer.WriteInt(value.pubAngebot_Garantiesumme);
		writer.WriteFloat(value.pubAngebot_Gewinnbeteiligung);
		writer.WriteBool(value.auftragsspiel);
		writer.WriteInt(value.auftragsspiel_gehalt);
		writer.WriteInt(value.auftragsspiel_bonus);
		writer.WriteInt(value.auftragsspiel_zeitInWochen);
		writer.WriteInt(value.auftragsspiel_wochenAlsAngebot);
		writer.WriteBool(value.auftragsspiel_zeitAbgelaufen);
		writer.WriteInt(value.auftragsspiel_mindestbewertung);
		writer.WriteBool(value.f2pConverted);
		writer.WriteBool(value.angekuendigt);
		writer.WriteLong(value.subvention);
		writer.WriteBool(value.sonderIP);
		writer.WriteInt(value.sonderIPMindestreview);
		writer.WriteString(value.myNameTeil1);
		writer.WriteInt(value.engineGewinnbeteiligung);
		writer.WriteInt(value.weeksInDevelopment);
		writer.WriteInt(value.userPositiv);
		writer.WriteInt(value.userNegativ);
		writer.WriteLong(value.bestAbonnements);
		writer.WriteInt(value.bestChartPosition);
		writer.WriteLong(value.exklusivKonsolenSells);
		writer.WriteInt(value.lastChartPosition);
		writer.WriteBool(value.freeware);
		writer.WriteLong(value.sellsTotalStandard);
		writer.WriteLong(value.sellsTotalDeluxe);
		writer.WriteLong(value.sellsTotalCollectors);
		writer.WriteLong(value.sellsTotalOnline);
		writer.WriteFloat(value.points_bugsInvis);
		writer.WriteLong(value.umsatzInApp);
		writer.WriteLong(value.umsatzAbos);
		writer.WriteFloat(value.f2pInteresse);
		writer.WriteFloat(value.mmoInteresse);
		writer.WriteInt(value.vorbestellungen);
		writer.WriteFloat(value.realsticPower);
		writer.WriteInt(value.stornierungen);
		writer.WriteBool(value.commercialFlop);
		writer.WriteBool(value.commercialHit);
		writer.WriteInt(value.inAppPurchaseWeek);
		writer.WriteInt(value.arcadeCase);
		writer.WriteInt(value.arcadeMonitor);
		writer.WriteInt(value.arcadeJoystick);
		writer.WriteInt(value.arcadeSound);
		writer.WriteInt(value.arcadeProdCosts);
		writer.WriteInt(value.finanzierung_Grundkosten);
		writer.WriteInt(value.finanzierung_Technology);
		writer.WriteInt(value.finanzierung_Kontent);
		writer.WriteBool(value.retailVersion);
		writer.WriteBool(value.digitalVersion);
		writer.WriteBool(value.newGenreCombination);
		writer.WriteBool(value.newTopicCombination);
		writer.WriteInt(value.ipTime);
		writer.WriteBool(value.npcLateinNumbers);
		writer.WriteBool(value.mmoTOf2p_created);
		writer.WriteBool(value.bundle_created);
		writer.WriteInt(value.abosAddons);
		_Write_System_002EBoolean_005B_005D(writer, value.inAppPurchase);
		_Write_System_002EInt32_005B_005D(writer, value.Designschwerpunkt);
		_Write_System_002EInt32_005B_005D(writer, value.Designausrichtung);
	}

	public static mpCalls.s_AddPlayer _Read_mpCalls_002Fs_AddPlayer(NetworkReader reader)
	{
		return new mpCalls.s_AddPlayer
		{
			playerID = reader.ReadInt()
		};
	}

	public static void _Write_mpCalls_002Fs_AddPlayer(NetworkWriter writer, mpCalls.s_AddPlayer value)
	{
		writer.WriteInt(value.playerID);
	}

	public static mpCalls.s_Forschung _Read_mpCalls_002Fs_Forschung(NetworkReader reader)
	{
		return new mpCalls.s_Forschung
		{
			playerID = reader.ReadInt(),
			forschungSonstiges = _Read_System_002EBoolean_005B_005D(reader),
			genres = _Read_System_002EBoolean_005B_005D(reader),
			themes = _Read_System_002EBoolean_005B_005D(reader),
			engineFeatures = _Read_System_002EBoolean_005B_005D(reader),
			gameplayFeatures = _Read_System_002EBoolean_005B_005D(reader),
			hardware = _Read_System_002EBoolean_005B_005D(reader),
			hardwareFeatures = _Read_System_002EBoolean_005B_005D(reader)
		};
	}

	public static void _Write_mpCalls_002Fs_Forschung(NetworkWriter writer, mpCalls.s_Forschung value)
	{
		writer.WriteInt(value.playerID);
		_Write_System_002EBoolean_005B_005D(writer, value.forschungSonstiges);
		_Write_System_002EBoolean_005B_005D(writer, value.genres);
		_Write_System_002EBoolean_005B_005D(writer, value.themes);
		_Write_System_002EBoolean_005B_005D(writer, value.engineFeatures);
		_Write_System_002EBoolean_005B_005D(writer, value.gameplayFeatures);
		_Write_System_002EBoolean_005B_005D(writer, value.hardware);
		_Write_System_002EBoolean_005B_005D(writer, value.hardwareFeatures);
	}

	public static mpCalls.s_PlayerLeave _Read_mpCalls_002Fs_PlayerLeave(NetworkReader reader)
	{
		return new mpCalls.s_PlayerLeave
		{
			playerID = reader.ReadInt()
		};
	}

	public static void _Write_mpCalls_002Fs_PlayerLeave(NetworkWriter writer, mpCalls.s_PlayerLeave value)
	{
		writer.WriteInt(value.playerID);
	}

	public static mpCalls.s_GenreBeliebtheit _Read_mpCalls_002Fs_GenreBeliebtheit(NetworkReader reader)
	{
		return new mpCalls.s_GenreBeliebtheit
		{
			genreBeliebtheit = _Read_System_002ESingle_005B_005D(reader)
		};
	}

	public static float[] _Read_System_002ESingle_005B_005D(NetworkReader reader)
	{
		return reader.ReadArray<float>();
	}

	public static void _Write_mpCalls_002Fs_GenreBeliebtheit(NetworkWriter writer, mpCalls.s_GenreBeliebtheit value)
	{
		_Write_System_002ESingle_005B_005D(writer, value.genreBeliebtheit);
	}

	public static void _Write_System_002ESingle_005B_005D(NetworkWriter writer, float[] value)
	{
		writer.WriteArray(value);
	}

	public static mpCalls.s_GenreCombination _Read_mpCalls_002Fs_GenreCombination(NetworkReader reader)
	{
		return new mpCalls.s_GenreCombination
		{
			genreSlot = reader.ReadInt(),
			genres_COMBINATION = _Read_System_002EBoolean_005B_005D(reader)
		};
	}

	public static void _Write_mpCalls_002Fs_GenreCombination(NetworkWriter writer, mpCalls.s_GenreCombination value)
	{
		writer.WriteInt(value.genreSlot);
		_Write_System_002EBoolean_005B_005D(writer, value.genres_COMBINATION);
	}

	public static mpCalls.s_GenreDesign _Read_mpCalls_002Fs_GenreDesign(NetworkReader reader)
	{
		return new mpCalls.s_GenreDesign
		{
			genreSlot = reader.ReadInt(),
			genres_focus0 = reader.ReadInt(),
			genres_focus1 = reader.ReadInt(),
			genres_focus2 = reader.ReadInt(),
			genres_focus3 = reader.ReadInt(),
			genres_focus4 = reader.ReadInt(),
			genres_focus5 = reader.ReadInt(),
			genres_focus6 = reader.ReadInt(),
			genres_focus7 = reader.ReadInt(),
			genres_align0 = reader.ReadInt(),
			genres_align1 = reader.ReadInt(),
			genres_align2 = reader.ReadInt()
		};
	}

	public static void _Write_mpCalls_002Fs_GenreDesign(NetworkWriter writer, mpCalls.s_GenreDesign value)
	{
		writer.WriteInt(value.genreSlot);
		writer.WriteInt(value.genres_focus0);
		writer.WriteInt(value.genres_focus1);
		writer.WriteInt(value.genres_focus2);
		writer.WriteInt(value.genres_focus3);
		writer.WriteInt(value.genres_focus4);
		writer.WriteInt(value.genres_focus5);
		writer.WriteInt(value.genres_focus6);
		writer.WriteInt(value.genres_focus7);
		writer.WriteInt(value.genres_align0);
		writer.WriteInt(value.genres_align1);
		writer.WriteInt(value.genres_align2);
	}

	public static mpCalls.s_GenrePlatformSuit _Read_mpCalls_002Fs_GenrePlatformSuit(NetworkReader reader)
	{
		return new mpCalls.s_GenrePlatformSuit
		{
			genreSlot = reader.ReadInt(),
			pc_0 = reader.ReadInt(),
			konsole_1 = reader.ReadInt(),
			handheld_2 = reader.ReadInt(),
			handy_3 = reader.ReadInt(),
			arcade_4 = reader.ReadInt()
		};
	}

	public static void _Write_mpCalls_002Fs_GenrePlatformSuit(NetworkWriter writer, mpCalls.s_GenrePlatformSuit value)
	{
		writer.WriteInt(value.genreSlot);
		writer.WriteInt(value.pc_0);
		writer.WriteInt(value.konsole_1);
		writer.WriteInt(value.handheld_2);
		writer.WriteInt(value.handy_3);
		writer.WriteInt(value.arcade_4);
	}

	public static mpCalls.s_Help _Read_mpCalls_002Fs_Help(NetworkReader reader)
	{
		return new mpCalls.s_Help
		{
			playerID = reader.ReadInt(),
			toPlayerID = reader.ReadInt(),
			what = reader.ReadInt(),
			valueA = reader.ReadInt(),
			valueB = reader.ReadInt(),
			valueC = reader.ReadInt()
		};
	}

	public static void _Write_mpCalls_002Fs_Help(NetworkWriter writer, mpCalls.s_Help value)
	{
		writer.WriteInt(value.playerID);
		writer.WriteInt(value.toPlayerID);
		writer.WriteInt(value.what);
		writer.WriteInt(value.valueA);
		writer.WriteInt(value.valueB);
		writer.WriteInt(value.valueC);
	}

	public static mpCalls.s_ObjectDelete _Read_mpCalls_002Fs_ObjectDelete(NetworkReader reader)
	{
		return new mpCalls.s_ObjectDelete
		{
			playerID = reader.ReadInt(),
			objectID = reader.ReadInt()
		};
	}

	public static void _Write_mpCalls_002Fs_ObjectDelete(NetworkWriter writer, mpCalls.s_ObjectDelete value)
	{
		writer.WriteInt(value.playerID);
		writer.WriteInt(value.objectID);
	}

	public static mpCalls.s_Object _Read_mpCalls_002Fs_Object(NetworkReader reader)
	{
		return new mpCalls.s_Object
		{
			playerID = reader.ReadInt(),
			objectID = reader.ReadInt(),
			typ = reader.ReadInt(),
			x = reader.ReadFloat(),
			y = reader.ReadFloat(),
			rot = reader.ReadFloat()
		};
	}

	public static void _Write_mpCalls_002Fs_Object(NetworkWriter writer, mpCalls.s_Object value)
	{
		writer.WriteInt(value.playerID);
		writer.WriteInt(value.objectID);
		writer.WriteInt(value.typ);
		writer.WriteFloat(value.x);
		writer.WriteFloat(value.y);
		writer.WriteFloat(value.rot);
	}

	public static mpCalls.s_Map _Read_mpCalls_002Fs_Map(NetworkReader reader)
	{
		return new mpCalls.s_Map
		{
			playerID = reader.ReadInt(),
			x = NetworkReaderExtensions.ReadByte(reader),
			y = NetworkReaderExtensions.ReadByte(reader),
			id = reader.ReadInt(),
			typ = reader.ReadInt(),
			door = reader.ReadInt(),
			window = reader.ReadInt()
		};
	}

	public static void _Write_mpCalls_002Fs_Map(NetworkWriter writer, mpCalls.s_Map value)
	{
		writer.WriteInt(value.playerID);
		NetworkWriterExtensions.WriteByte(writer, value.x);
		NetworkWriterExtensions.WriteByte(writer, value.y);
		writer.WriteInt(value.id);
		writer.WriteInt(value.typ);
		writer.WriteInt(value.door);
		writer.WriteInt(value.window);
	}

	public static mpCalls.s_Office _Read_mpCalls_002Fs_Office(NetworkReader reader)
	{
		return new mpCalls.s_Office
		{
			office = reader.ReadInt()
		};
	}

	public static void _Write_mpCalls_002Fs_Office(NetworkWriter writer, mpCalls.s_Office value)
	{
		writer.WriteInt(value.office);
	}

	public static mpCalls.s_Difficulty _Read_mpCalls_002Fs_Difficulty(NetworkReader reader)
	{
		return new mpCalls.s_Difficulty
		{
			difficulty = reader.ReadInt()
		};
	}

	public static void _Write_mpCalls_002Fs_Difficulty(NetworkWriter writer, mpCalls.s_Difficulty value)
	{
		writer.WriteInt(value.difficulty);
	}

	public static mpCalls.s_RandomSettings _Read_mpCalls_002Fs_RandomSettings(NetworkReader reader)
	{
		return new mpCalls.s_RandomSettings
		{
			randomSettings = reader.ReadInt()
		};
	}

	public static void _Write_mpCalls_002Fs_RandomSettings(NetworkWriter writer, mpCalls.s_RandomSettings value)
	{
		writer.WriteInt(value.randomSettings);
	}

	public static mpCalls.s_Wettbewerb _Read_mpCalls_002Fs_Wettbewerb(NetworkReader reader)
	{
		return new mpCalls.s_Wettbewerb
		{
			competition = reader.ReadInt(),
			settings_RandomReviewsNum = reader.ReadInt(),
			settings_randomPlattformNum = reader.ReadInt()
		};
	}

	public static void _Write_mpCalls_002Fs_Wettbewerb(NetworkWriter writer, mpCalls.s_Wettbewerb value)
	{
		writer.WriteInt(value.competition);
		writer.WriteInt(value.settings_RandomReviewsNum);
		writer.WriteInt(value.settings_randomPlattformNum);
	}

	public static mpCalls.s_Startjahr _Read_mpCalls_002Fs_Startjahr(NetworkReader reader)
	{
		return new mpCalls.s_Startjahr
		{
			startjahr = reader.ReadInt()
		};
	}

	public static void _Write_mpCalls_002Fs_Startjahr(NetworkWriter writer, mpCalls.s_Startjahr value)
	{
		writer.WriteInt(value.startjahr);
	}

	public static mpCalls.s_Entwicklungsdauer _Read_mpCalls_002Fs_Entwicklungsdauer(NetworkReader reader)
	{
		return new mpCalls.s_Entwicklungsdauer
		{
			dauer = reader.ReadInt()
		};
	}

	public static void _Write_mpCalls_002Fs_Entwicklungsdauer(NetworkWriter writer, mpCalls.s_Entwicklungsdauer value)
	{
		writer.WriteInt(value.dauer);
	}

	public static mpCalls.s_AnzahlKonkurrenten _Read_mpCalls_002Fs_AnzahlKonkurrenten(NetworkReader reader)
	{
		return new mpCalls.s_AnzahlKonkurrenten
		{
			anzahl = reader.ReadInt()
		};
	}

	public static void _Write_mpCalls_002Fs_AnzahlKonkurrenten(NetworkWriter writer, mpCalls.s_AnzahlKonkurrenten value)
	{
		writer.WriteInt(value.anzahl);
	}

	public static mpCalls.s_Spielgeschwindigkeit _Read_mpCalls_002Fs_Spielgeschwindigkeit(NetworkReader reader)
	{
		return new mpCalls.s_Spielgeschwindigkeit
		{
			gamespeed = reader.ReadInt()
		};
	}

	public static void _Write_mpCalls_002Fs_Spielgeschwindigkeit(NetworkWriter writer, mpCalls.s_Spielgeschwindigkeit value)
	{
		writer.WriteInt(value.gamespeed);
	}

	public static mpCalls.s_GlobalEvent _Read_mpCalls_002Fs_GlobalEvent(NetworkReader reader)
	{
		return new mpCalls.s_GlobalEvent
		{
			eventID = reader.ReadInt(),
			wochen = reader.ReadInt()
		};
	}

	public static void _Write_mpCalls_002Fs_GlobalEvent(NetworkWriter writer, mpCalls.s_GlobalEvent value)
	{
		writer.WriteInt(value.eventID);
		writer.WriteInt(value.wochen);
	}

	public static mpCalls.s_EngineAbrechnung _Read_mpCalls_002Fs_EngineAbrechnung(NetworkReader reader)
	{
		return new mpCalls.s_EngineAbrechnung
		{
			toPlayerID = reader.ReadInt(),
			gameID = reader.ReadInt()
		};
	}

	public static void _Write_mpCalls_002Fs_EngineAbrechnung(NetworkWriter writer, mpCalls.s_EngineAbrechnung value)
	{
		writer.WriteInt(value.toPlayerID);
		writer.WriteInt(value.gameID);
	}

	public static mpCalls.s_Awards _Read_mpCalls_002Fs_Awards(NetworkReader reader)
	{
		return new mpCalls.s_Awards
		{
			bestGrafik = reader.ReadInt(),
			bestSound = reader.ReadInt(),
			bestStudio = reader.ReadInt(),
			bestPublisher = reader.ReadInt(),
			bestGame = reader.ReadInt(),
			badGame = reader.ReadInt()
		};
	}

	public static void _Write_mpCalls_002Fs_Awards(NetworkWriter writer, mpCalls.s_Awards value)
	{
		writer.WriteInt(value.bestGrafik);
		writer.WriteInt(value.bestSound);
		writer.WriteInt(value.bestStudio);
		writer.WriteInt(value.bestPublisher);
		writer.WriteInt(value.bestGame);
		writer.WriteInt(value.badGame);
	}

	public static mpCalls.s_Payment _Read_mpCalls_002Fs_Payment(NetworkReader reader)
	{
		return new mpCalls.s_Payment
		{
			playerID = reader.ReadInt(),
			toPlayerID = reader.ReadInt(),
			what = reader.ReadInt(),
			money = reader.ReadInt(),
			objectID = reader.ReadInt()
		};
	}

	public static void _Write_mpCalls_002Fs_Payment(NetworkWriter writer, mpCalls.s_Payment value)
	{
		writer.WriteInt(value.playerID);
		writer.WriteInt(value.toPlayerID);
		writer.WriteInt(value.what);
		writer.WriteInt(value.money);
		writer.WriteInt(value.objectID);
	}

	public static mpCalls.s_Engine _Read_mpCalls_002Fs_Engine(NetworkReader reader)
	{
		return new mpCalls.s_Engine
		{
			engineID = reader.ReadInt(),
			ownerID = reader.ReadInt(),
			isUnlocked = reader.ReadBool(),
			gekauft = reader.ReadBool(),
			myName = reader.ReadString(),
			features = _Read_System_002EBoolean_005B_005D(reader),
			spezialgenre = reader.ReadInt(),
			spezialplatform = reader.ReadInt(),
			sellEngine = reader.ReadBool(),
			preis = reader.ReadInt(),
			gewinnbeteiligung = reader.ReadInt(),
			marktdominanz = reader.ReadFloat()
		};
	}

	public static void _Write_mpCalls_002Fs_Engine(NetworkWriter writer, mpCalls.s_Engine value)
	{
		writer.WriteInt(value.engineID);
		writer.WriteInt(value.ownerID);
		writer.WriteBool(value.isUnlocked);
		writer.WriteBool(value.gekauft);
		writer.WriteString(value.myName);
		_Write_System_002EBoolean_005B_005D(writer, value.features);
		writer.WriteInt(value.spezialgenre);
		writer.WriteInt(value.spezialplatform);
		writer.WriteBool(value.sellEngine);
		writer.WriteInt(value.preis);
		writer.WriteInt(value.gewinnbeteiligung);
		writer.WriteFloat(value.marktdominanz);
	}

	public static mpCalls.s_EnginePublisherBuyed _Read_mpCalls_002Fs_EnginePublisherBuyed(NetworkReader reader)
	{
		return new mpCalls.s_EnginePublisherBuyed
		{
			engineID = reader.ReadInt(),
			publisherBuyed = _Read_System_002EBoolean_005B_005D(reader)
		};
	}

	public static void _Write_mpCalls_002Fs_EnginePublisherBuyed(NetworkWriter writer, mpCalls.s_EnginePublisherBuyed value)
	{
		writer.WriteInt(value.engineID);
		_Write_System_002EBoolean_005B_005D(writer, value.publisherBuyed);
	}

	public static mpCalls.s_Platform _Read_mpCalls_002Fs_Platform(NetworkReader reader)
	{
		return new mpCalls.s_Platform
		{
			myID = reader.ReadInt(),
			date_year = reader.ReadInt(),
			date_month = reader.ReadInt(),
			date_year_end = reader.ReadInt(),
			date_month_end = reader.ReadInt(),
			price = reader.ReadInt(),
			dev_costs = reader.ReadInt(),
			tech = reader.ReadInt(),
			typ = reader.ReadInt(),
			marktanteil = reader.ReadFloat(),
			needFeatures = _Read_System_002EInt32_005B_005D(reader),
			units = reader.ReadInt(),
			units_max = reader.ReadInt(),
			minGamePassGames = reader.ReadInt(),
			name_EN = reader.ReadString(),
			name_GE = reader.ReadString(),
			name_TU = reader.ReadString(),
			name_CH = reader.ReadString(),
			name_FR = reader.ReadString(),
			name_HU = reader.ReadString(),
			name_JA = reader.ReadString(),
			name_PL = reader.ReadString(),
			name_UA = reader.ReadString(),
			name_TH = reader.ReadString(),
			manufacturer_EN = reader.ReadString(),
			manufacturer_GE = reader.ReadString(),
			manufacturer_TU = reader.ReadString(),
			manufacturer_CH = reader.ReadString(),
			manufacturer_FR = reader.ReadString(),
			manufacturer_HU = reader.ReadString(),
			manufacturer_JA = reader.ReadString(),
			manufacturer_PL = reader.ReadString(),
			manufacturer_UA = reader.ReadString(),
			manufacturer_TH = reader.ReadString(),
			pic1_file = reader.ReadString(),
			pic2_file = reader.ReadString(),
			pic2_year = reader.ReadInt(),
			games = reader.ReadInt(),
			exklusivGames = reader.ReadInt(),
			erfahrung = reader.ReadInt(),
			isUnlocked = reader.ReadBool(),
			inBesitz = reader.ReadBool(),
			vomMarktGenommen = reader.ReadBool(),
			complex = reader.ReadInt(),
			internet = reader.ReadBool(),
			powerFromMarket = reader.ReadFloat(),
			myName = reader.ReadString(),
			ownerID = reader.ReadInt(),
			gameID = reader.ReadInt(),
			anzController = reader.ReadInt(),
			consoleColor = reader.ReadVector3(),
			component_cpu = reader.ReadInt(),
			component_gfx = reader.ReadInt(),
			component_ram = reader.ReadInt(),
			component_hdd = reader.ReadInt(),
			component_sfx = reader.ReadInt(),
			component_cooling = reader.ReadInt(),
			component_disc = reader.ReadInt(),
			component_controller = reader.ReadInt(),
			component_case = reader.ReadInt(),
			component_monitor = reader.ReadInt(),
			hwFeatures = _Read_System_002EBoolean_005B_005D(reader),
			devPoints = reader.ReadFloat(),
			devPointsStart = reader.ReadFloat(),
			entwicklungsKosten = reader.ReadLong(),
			einnahmen = reader.ReadLong(),
			hype = reader.ReadFloat(),
			startProduktionskosten = reader.ReadInt(),
			verkaufspreis = reader.ReadInt(),
			kostenreduktion = reader.ReadFloat(),
			autoPreis = reader.ReadBool(),
			thridPartyGames = reader.ReadBool(),
			umsatzTotal = reader.ReadLong(),
			sellsPerWeek = _Read_System_002EInt32_005B_005D(reader),
			weeksOnMarket = reader.ReadInt(),
			review = reader.ReadFloat(),
			performancePoints = reader.ReadInt(),
			nachfolgerID = reader.ReadInt(),
			vorgaengerID = reader.ReadInt(),
			proVersion = reader.ReadBool(),
			proName = reader.ReadString(),
			subventionMoney = reader.ReadInt(),
			subventionGameSize = _Read_System_002EBoolean_005B_005D(reader)
		};
	}

	public static void _Write_mpCalls_002Fs_Platform(NetworkWriter writer, mpCalls.s_Platform value)
	{
		writer.WriteInt(value.myID);
		writer.WriteInt(value.date_year);
		writer.WriteInt(value.date_month);
		writer.WriteInt(value.date_year_end);
		writer.WriteInt(value.date_month_end);
		writer.WriteInt(value.price);
		writer.WriteInt(value.dev_costs);
		writer.WriteInt(value.tech);
		writer.WriteInt(value.typ);
		writer.WriteFloat(value.marktanteil);
		_Write_System_002EInt32_005B_005D(writer, value.needFeatures);
		writer.WriteInt(value.units);
		writer.WriteInt(value.units_max);
		writer.WriteInt(value.minGamePassGames);
		writer.WriteString(value.name_EN);
		writer.WriteString(value.name_GE);
		writer.WriteString(value.name_TU);
		writer.WriteString(value.name_CH);
		writer.WriteString(value.name_FR);
		writer.WriteString(value.name_HU);
		writer.WriteString(value.name_JA);
		writer.WriteString(value.name_PL);
		writer.WriteString(value.name_UA);
		writer.WriteString(value.name_TH);
		writer.WriteString(value.manufacturer_EN);
		writer.WriteString(value.manufacturer_GE);
		writer.WriteString(value.manufacturer_TU);
		writer.WriteString(value.manufacturer_CH);
		writer.WriteString(value.manufacturer_FR);
		writer.WriteString(value.manufacturer_HU);
		writer.WriteString(value.manufacturer_JA);
		writer.WriteString(value.manufacturer_PL);
		writer.WriteString(value.manufacturer_UA);
		writer.WriteString(value.manufacturer_TH);
		writer.WriteString(value.pic1_file);
		writer.WriteString(value.pic2_file);
		writer.WriteInt(value.pic2_year);
		writer.WriteInt(value.games);
		writer.WriteInt(value.exklusivGames);
		writer.WriteInt(value.erfahrung);
		writer.WriteBool(value.isUnlocked);
		writer.WriteBool(value.inBesitz);
		writer.WriteBool(value.vomMarktGenommen);
		writer.WriteInt(value.complex);
		writer.WriteBool(value.internet);
		writer.WriteFloat(value.powerFromMarket);
		writer.WriteString(value.myName);
		writer.WriteInt(value.ownerID);
		writer.WriteInt(value.gameID);
		writer.WriteInt(value.anzController);
		writer.WriteVector3(value.consoleColor);
		writer.WriteInt(value.component_cpu);
		writer.WriteInt(value.component_gfx);
		writer.WriteInt(value.component_ram);
		writer.WriteInt(value.component_hdd);
		writer.WriteInt(value.component_sfx);
		writer.WriteInt(value.component_cooling);
		writer.WriteInt(value.component_disc);
		writer.WriteInt(value.component_controller);
		writer.WriteInt(value.component_case);
		writer.WriteInt(value.component_monitor);
		_Write_System_002EBoolean_005B_005D(writer, value.hwFeatures);
		writer.WriteFloat(value.devPoints);
		writer.WriteFloat(value.devPointsStart);
		writer.WriteLong(value.entwicklungsKosten);
		writer.WriteLong(value.einnahmen);
		writer.WriteFloat(value.hype);
		writer.WriteInt(value.startProduktionskosten);
		writer.WriteInt(value.verkaufspreis);
		writer.WriteFloat(value.kostenreduktion);
		writer.WriteBool(value.autoPreis);
		writer.WriteBool(value.thridPartyGames);
		writer.WriteLong(value.umsatzTotal);
		_Write_System_002EInt32_005B_005D(writer, value.sellsPerWeek);
		writer.WriteInt(value.weeksOnMarket);
		writer.WriteFloat(value.review);
		writer.WriteInt(value.performancePoints);
		writer.WriteInt(value.nachfolgerID);
		writer.WriteInt(value.vorgaengerID);
		writer.WriteBool(value.proVersion);
		writer.WriteString(value.proName);
		writer.WriteInt(value.subventionMoney);
		_Write_System_002EBoolean_005B_005D(writer, value.subventionGameSize);
	}

	public static mpCalls.s_PlatformData _Read_mpCalls_002Fs_PlatformData(NetworkReader reader)
	{
		return new mpCalls.s_PlatformData
		{
			platformID = reader.ReadInt(),
			marktanteil = reader.ReadFloat(),
			units = reader.ReadInt(),
			units_max = reader.ReadInt(),
			date_year_end = reader.ReadInt()
		};
	}

	public static void _Write_mpCalls_002Fs_PlatformData(NetworkWriter writer, mpCalls.s_PlatformData value)
	{
		writer.WriteInt(value.platformID);
		writer.WriteFloat(value.marktanteil);
		writer.WriteInt(value.units);
		writer.WriteInt(value.units_max);
		writer.WriteInt(value.date_year_end);
	}

	public static mpCalls.s_PlatformRemoveFromMarket _Read_mpCalls_002Fs_PlatformRemoveFromMarket(NetworkReader reader)
	{
		return new mpCalls.s_PlatformRemoveFromMarket
		{
			platformID = reader.ReadInt()
		};
	}

	public static void _Write_mpCalls_002Fs_PlatformRemoveFromMarket(NetworkWriter writer, mpCalls.s_PlatformRemoveFromMarket value)
	{
		writer.WriteInt(value.platformID);
	}

	public static mpCalls.s_PlatformSubvention _Read_mpCalls_002Fs_PlatformSubvention(NetworkReader reader)
	{
		return new mpCalls.s_PlatformSubvention
		{
			platformID = reader.ReadInt(),
			gameID = reader.ReadInt(),
			subvention = reader.ReadInt()
		};
	}

	public static void _Write_mpCalls_002Fs_PlatformSubvention(NetworkWriter writer, mpCalls.s_PlatformSubvention value)
	{
		writer.WriteInt(value.platformID);
		writer.WriteInt(value.gameID);
		writer.WriteInt(value.subvention);
	}

	public static mpCalls.s_Chat _Read_mpCalls_002Fs_Chat(NetworkReader reader)
	{
		return new mpCalls.s_Chat
		{
			playerID = reader.ReadInt(),
			text = reader.ReadString()
		};
	}

	public static void _Write_mpCalls_002Fs_Chat(NetworkWriter writer, mpCalls.s_Chat value)
	{
		writer.WriteInt(value.playerID);
		writer.WriteString(value.text);
	}

	public static mpCalls.s_Money _Read_mpCalls_002Fs_Money(NetworkReader reader)
	{
		return new mpCalls.s_Money
		{
			playerID = reader.ReadInt(),
			money = reader.ReadLong(),
			fans = reader.ReadInt()
		};
	}

	public static void _Write_mpCalls_002Fs_Money(NetworkWriter writer, mpCalls.s_Money value)
	{
		writer.WriteInt(value.playerID);
		writer.WriteLong(value.money);
		writer.WriteInt(value.fans);
	}

	public static mpCalls.s_AutoPause _Read_mpCalls_002Fs_AutoPause(NetworkReader reader)
	{
		return new mpCalls.s_AutoPause
		{
			playerID = reader.ReadInt(),
			pause = reader.ReadBool()
		};
	}

	public static void _Write_mpCalls_002Fs_AutoPause(NetworkWriter writer, mpCalls.s_AutoPause value)
	{
		writer.WriteInt(value.playerID);
		writer.WriteBool(value.pause);
	}

	public static mpCalls.s_Genres _Read_mpCalls_002Fs_Genres(NetworkReader reader)
	{
		return new mpCalls.s_Genres
		{
			genres_BELIEBTHEIT = _Read_System_002ESingle_005B_005D(reader),
			genres_BELIEBTHEIT_SOLL = _Read_System_002EBoolean_005B_005D(reader),
			genres_RES_POINTS = _Read_System_002EInt32_005B_005D(reader),
			genres_RES_POINTS_LEFT = _Read_System_002ESingle_005B_005D(reader),
			genres_PRICE = _Read_System_002EInt32_005B_005D(reader),
			genres_DEV_COSTS = _Read_System_002EInt32_005B_005D(reader),
			genres_DATE_YEAR = _Read_System_002EInt32_005B_005D(reader),
			genres_DATE_MONTH = _Read_System_002EInt32_005B_005D(reader),
			genres_LEVEL = _Read_System_002EInt32_005B_005D(reader),
			genres_UNLOCK = _Read_System_002EBoolean_005B_005D(reader),
			genres_SUC_YEAR = _Read_System_002EBoolean_005B_005D(reader),
			genres_TARGETGROUP = _Read_System_002EBoolean_005B_005D(reader),
			genres_GAMEPLAY = _Read_System_002ESingle_005B_005D(reader),
			genres_GRAPHIC = _Read_System_002ESingle_005B_005D(reader),
			genres_SOUND = _Read_System_002ESingle_005B_005D(reader),
			genres_CONTROL = _Read_System_002ESingle_005B_005D(reader),
			genres_COMBINATION = _Read_System_002EBoolean_005B_005D(reader),
			genres_PLATFORM_SELLS = _Read_System_002EInt32_005B_005D(reader),
			genres_FOCUS = _Read_System_002EInt32_005B_005D(reader),
			genres_FOCUS_KNOWN = _Read_System_002EBoolean_005B_005D(reader),
			genres_ALIGN = _Read_System_002EInt32_005B_005D(reader),
			genres_ALIGN_KNOWN = _Read_System_002EBoolean_005B_005D(reader),
			genres_ICONFILE = _Read_System_002EString_005B_005D(reader),
			genres_NAME_EN = _Read_System_002EString_005B_005D(reader),
			genres_NAME_GE = _Read_System_002EString_005B_005D(reader),
			genres_NAME_TU = _Read_System_002EString_005B_005D(reader),
			genres_NAME_CH = _Read_System_002EString_005B_005D(reader),
			genres_NAME_FR = _Read_System_002EString_005B_005D(reader),
			genres_NAME_PB = _Read_System_002EString_005B_005D(reader),
			genres_NAME_HU = _Read_System_002EString_005B_005D(reader),
			genres_NAME_CT = _Read_System_002EString_005B_005D(reader),
			genres_NAME_ES = _Read_System_002EString_005B_005D(reader),
			genres_NAME_PL = _Read_System_002EString_005B_005D(reader),
			genres_NAME_CZ = _Read_System_002EString_005B_005D(reader),
			genres_NAME_KO = _Read_System_002EString_005B_005D(reader),
			genres_NAME_IT = _Read_System_002EString_005B_005D(reader),
			genres_NAME_AR = _Read_System_002EString_005B_005D(reader),
			genres_NAME_JA = _Read_System_002EString_005B_005D(reader),
			genres_NAME_UA = _Read_System_002EString_005B_005D(reader),
			genres_NAME_TH = _Read_System_002EString_005B_005D(reader),
			genres_NAME_RU = _Read_System_002EString_005B_005D(reader),
			genres_DESC_EN = _Read_System_002EString_005B_005D(reader),
			genres_DESC_GE = _Read_System_002EString_005B_005D(reader),
			genres_DESC_TU = _Read_System_002EString_005B_005D(reader),
			genres_DESC_CH = _Read_System_002EString_005B_005D(reader),
			genres_DESC_FR = _Read_System_002EString_005B_005D(reader),
			genres_DESC_PB = _Read_System_002EString_005B_005D(reader),
			genres_DESC_HU = _Read_System_002EString_005B_005D(reader),
			genres_DESC_CT = _Read_System_002EString_005B_005D(reader),
			genres_DESC_ES = _Read_System_002EString_005B_005D(reader),
			genres_DESC_PL = _Read_System_002EString_005B_005D(reader),
			genres_DESC_CZ = _Read_System_002EString_005B_005D(reader),
			genres_DESC_KO = _Read_System_002EString_005B_005D(reader),
			genres_DESC_IT = _Read_System_002EString_005B_005D(reader),
			genres_DESC_AR = _Read_System_002EString_005B_005D(reader),
			genres_DESC_JA = _Read_System_002EString_005B_005D(reader),
			genres_DESC_UA = _Read_System_002EString_005B_005D(reader),
			genres_DESC_TH = _Read_System_002EString_005B_005D(reader),
			genres_DESC_RU = _Read_System_002EString_005B_005D(reader),
			genres_FANS = _Read_System_002EInt32_005B_005D(reader),
			genres_MARKT = _Read_System_002EInt32_005B_005D(reader)
		};
	}

	public static string[] _Read_System_002EString_005B_005D(NetworkReader reader)
	{
		return reader.ReadArray<string>();
	}

	public static void _Write_mpCalls_002Fs_Genres(NetworkWriter writer, mpCalls.s_Genres value)
	{
		_Write_System_002ESingle_005B_005D(writer, value.genres_BELIEBTHEIT);
		_Write_System_002EBoolean_005B_005D(writer, value.genres_BELIEBTHEIT_SOLL);
		_Write_System_002EInt32_005B_005D(writer, value.genres_RES_POINTS);
		_Write_System_002ESingle_005B_005D(writer, value.genres_RES_POINTS_LEFT);
		_Write_System_002EInt32_005B_005D(writer, value.genres_PRICE);
		_Write_System_002EInt32_005B_005D(writer, value.genres_DEV_COSTS);
		_Write_System_002EInt32_005B_005D(writer, value.genres_DATE_YEAR);
		_Write_System_002EInt32_005B_005D(writer, value.genres_DATE_MONTH);
		_Write_System_002EInt32_005B_005D(writer, value.genres_LEVEL);
		_Write_System_002EBoolean_005B_005D(writer, value.genres_UNLOCK);
		_Write_System_002EBoolean_005B_005D(writer, value.genres_SUC_YEAR);
		_Write_System_002EBoolean_005B_005D(writer, value.genres_TARGETGROUP);
		_Write_System_002ESingle_005B_005D(writer, value.genres_GAMEPLAY);
		_Write_System_002ESingle_005B_005D(writer, value.genres_GRAPHIC);
		_Write_System_002ESingle_005B_005D(writer, value.genres_SOUND);
		_Write_System_002ESingle_005B_005D(writer, value.genres_CONTROL);
		_Write_System_002EBoolean_005B_005D(writer, value.genres_COMBINATION);
		_Write_System_002EInt32_005B_005D(writer, value.genres_PLATFORM_SELLS);
		_Write_System_002EInt32_005B_005D(writer, value.genres_FOCUS);
		_Write_System_002EBoolean_005B_005D(writer, value.genres_FOCUS_KNOWN);
		_Write_System_002EInt32_005B_005D(writer, value.genres_ALIGN);
		_Write_System_002EBoolean_005B_005D(writer, value.genres_ALIGN_KNOWN);
		_Write_System_002EString_005B_005D(writer, value.genres_ICONFILE);
		_Write_System_002EString_005B_005D(writer, value.genres_NAME_EN);
		_Write_System_002EString_005B_005D(writer, value.genres_NAME_GE);
		_Write_System_002EString_005B_005D(writer, value.genres_NAME_TU);
		_Write_System_002EString_005B_005D(writer, value.genres_NAME_CH);
		_Write_System_002EString_005B_005D(writer, value.genres_NAME_FR);
		_Write_System_002EString_005B_005D(writer, value.genres_NAME_PB);
		_Write_System_002EString_005B_005D(writer, value.genres_NAME_HU);
		_Write_System_002EString_005B_005D(writer, value.genres_NAME_CT);
		_Write_System_002EString_005B_005D(writer, value.genres_NAME_ES);
		_Write_System_002EString_005B_005D(writer, value.genres_NAME_PL);
		_Write_System_002EString_005B_005D(writer, value.genres_NAME_CZ);
		_Write_System_002EString_005B_005D(writer, value.genres_NAME_KO);
		_Write_System_002EString_005B_005D(writer, value.genres_NAME_IT);
		_Write_System_002EString_005B_005D(writer, value.genres_NAME_AR);
		_Write_System_002EString_005B_005D(writer, value.genres_NAME_JA);
		_Write_System_002EString_005B_005D(writer, value.genres_NAME_UA);
		_Write_System_002EString_005B_005D(writer, value.genres_NAME_TH);
		_Write_System_002EString_005B_005D(writer, value.genres_NAME_RU);
		_Write_System_002EString_005B_005D(writer, value.genres_DESC_EN);
		_Write_System_002EString_005B_005D(writer, value.genres_DESC_GE);
		_Write_System_002EString_005B_005D(writer, value.genres_DESC_TU);
		_Write_System_002EString_005B_005D(writer, value.genres_DESC_CH);
		_Write_System_002EString_005B_005D(writer, value.genres_DESC_FR);
		_Write_System_002EString_005B_005D(writer, value.genres_DESC_PB);
		_Write_System_002EString_005B_005D(writer, value.genres_DESC_HU);
		_Write_System_002EString_005B_005D(writer, value.genres_DESC_CT);
		_Write_System_002EString_005B_005D(writer, value.genres_DESC_ES);
		_Write_System_002EString_005B_005D(writer, value.genres_DESC_PL);
		_Write_System_002EString_005B_005D(writer, value.genres_DESC_CZ);
		_Write_System_002EString_005B_005D(writer, value.genres_DESC_KO);
		_Write_System_002EString_005B_005D(writer, value.genres_DESC_IT);
		_Write_System_002EString_005B_005D(writer, value.genres_DESC_AR);
		_Write_System_002EString_005B_005D(writer, value.genres_DESC_JA);
		_Write_System_002EString_005B_005D(writer, value.genres_DESC_UA);
		_Write_System_002EString_005B_005D(writer, value.genres_DESC_TH);
		_Write_System_002EString_005B_005D(writer, value.genres_DESC_RU);
		_Write_System_002EInt32_005B_005D(writer, value.genres_FANS);
		_Write_System_002EInt32_005B_005D(writer, value.genres_MARKT);
	}

	public static void _Write_System_002EString_005B_005D(NetworkWriter writer, string[] value)
	{
		writer.WriteArray(value);
	}

	public static mpCalls.s_Topics _Read_mpCalls_002Fs_Topics(NetworkReader reader)
	{
		return new mpCalls.s_Topics
		{
			RES_POINTS = reader.ReadInt(),
			themes_FITGENRE = _Read_System_002EBoolean_005B_005D(reader),
			themes_MGSR = _Read_System_002EInt32_005B_005D(reader)
		};
	}

	public static void _Write_mpCalls_002Fs_Topics(NetworkWriter writer, mpCalls.s_Topics value)
	{
		writer.WriteInt(value.RES_POINTS);
		_Write_System_002EBoolean_005B_005D(writer, value.themes_FITGENRE);
		_Write_System_002EInt32_005B_005D(writer, value.themes_MGSR);
	}

	public static mpCalls.s_GameplayFeatures _Read_mpCalls_002Fs_GameplayFeatures(NetworkReader reader)
	{
		return new mpCalls.s_GameplayFeatures
		{
			gameplayFeatures_TYP = _Read_System_002EInt32_005B_005D(reader),
			gameplayFeatures_RES_POINTS = _Read_System_002EInt32_005B_005D(reader),
			gameplayFeatures_RES_POINTS_LEFT = _Read_System_002ESingle_005B_005D(reader),
			gameplayFeatures_PRICE = _Read_System_002EInt32_005B_005D(reader),
			gameplayFeatures_DEV_COSTS = _Read_System_002EInt32_005B_005D(reader),
			gameplayFeatures_DATE_YEAR = _Read_System_002EInt32_005B_005D(reader),
			gameplayFeatures_DATE_MONTH = _Read_System_002EInt32_005B_005D(reader),
			gameplayFeatures_GAMEPLAY = _Read_System_002EInt32_005B_005D(reader),
			gameplayFeatures_GRAPHIC = _Read_System_002EInt32_005B_005D(reader),
			gameplayFeatures_SOUND = _Read_System_002EInt32_005B_005D(reader),
			gameplayFeatures_TECHNIK = _Read_System_002EInt32_005B_005D(reader),
			gameplayFeatures_LEVEL = _Read_System_002EInt32_005B_005D(reader),
			gameplayFeatures_NEED_GAMEPLAY_FEATURE = _Read_System_002EInt32_005B_005D(reader),
			gameplayFeatures_UNLOCK = _Read_System_002EBoolean_005B_005D(reader),
			gameplayFeatures_INTERNET = _Read_System_002EBoolean_005B_005D(reader),
			gameplayFeatures_ICONFILE = _Read_System_002EString_005B_005D(reader),
			gameplayFeatures_GOOD = _Read_System_002EBoolean_005B_005D(reader),
			gameplayFeatures_BAD = _Read_System_002EBoolean_005B_005D(reader),
			gameplayFeatures_LOCKPLATFORM = _Read_System_002EBoolean_005B_005D(reader),
			gameplayFeatures_NAME_EN = _Read_System_002EString_005B_005D(reader),
			gameplayFeatures_NAME_GE = _Read_System_002EString_005B_005D(reader),
			gameplayFeatures_NAME_TU = _Read_System_002EString_005B_005D(reader),
			gameplayFeatures_NAME_CH = _Read_System_002EString_005B_005D(reader),
			gameplayFeatures_NAME_FR = _Read_System_002EString_005B_005D(reader),
			gameplayFeatures_NAME_PB = _Read_System_002EString_005B_005D(reader),
			gameplayFeatures_NAME_CT = _Read_System_002EString_005B_005D(reader),
			gameplayFeatures_NAME_HU = _Read_System_002EString_005B_005D(reader),
			gameplayFeatures_NAME_ES = _Read_System_002EString_005B_005D(reader),
			gameplayFeatures_NAME_CZ = _Read_System_002EString_005B_005D(reader),
			gameplayFeatures_NAME_KO = _Read_System_002EString_005B_005D(reader),
			gameplayFeatures_NAME_RU = _Read_System_002EString_005B_005D(reader),
			gameplayFeatures_NAME_IT = _Read_System_002EString_005B_005D(reader),
			gameplayFeatures_NAME_AR = _Read_System_002EString_005B_005D(reader),
			gameplayFeatures_NAME_JA = _Read_System_002EString_005B_005D(reader),
			gameplayFeatures_NAME_PL = _Read_System_002EString_005B_005D(reader),
			gameplayFeatures_NAME_UA = _Read_System_002EString_005B_005D(reader),
			gameplayFeatures_NAME_TH = _Read_System_002EString_005B_005D(reader),
			gameplayFeatures_DESC_EN = _Read_System_002EString_005B_005D(reader),
			gameplayFeatures_DESC_GE = _Read_System_002EString_005B_005D(reader),
			gameplayFeatures_DESC_TU = _Read_System_002EString_005B_005D(reader),
			gameplayFeatures_DESC_CH = _Read_System_002EString_005B_005D(reader),
			gameplayFeatures_DESC_FR = _Read_System_002EString_005B_005D(reader),
			gameplayFeatures_DESC_PB = _Read_System_002EString_005B_005D(reader),
			gameplayFeatures_DESC_CT = _Read_System_002EString_005B_005D(reader),
			gameplayFeatures_DESC_HU = _Read_System_002EString_005B_005D(reader),
			gameplayFeatures_DESC_ES = _Read_System_002EString_005B_005D(reader),
			gameplayFeatures_DESC_CZ = _Read_System_002EString_005B_005D(reader),
			gameplayFeatures_DESC_KO = _Read_System_002EString_005B_005D(reader),
			gameplayFeatures_DESC_RU = _Read_System_002EString_005B_005D(reader),
			gameplayFeatures_DESC_IT = _Read_System_002EString_005B_005D(reader),
			gameplayFeatures_DESC_AR = _Read_System_002EString_005B_005D(reader),
			gameplayFeatures_DESC_JA = _Read_System_002EString_005B_005D(reader),
			gameplayFeatures_DESC_PL = _Read_System_002EString_005B_005D(reader),
			gameplayFeatures_DESC_UA = _Read_System_002EString_005B_005D(reader),
			gameplayFeatures_DESC_TH = _Read_System_002EString_005B_005D(reader)
		};
	}

	public static void _Write_mpCalls_002Fs_GameplayFeatures(NetworkWriter writer, mpCalls.s_GameplayFeatures value)
	{
		_Write_System_002EInt32_005B_005D(writer, value.gameplayFeatures_TYP);
		_Write_System_002EInt32_005B_005D(writer, value.gameplayFeatures_RES_POINTS);
		_Write_System_002ESingle_005B_005D(writer, value.gameplayFeatures_RES_POINTS_LEFT);
		_Write_System_002EInt32_005B_005D(writer, value.gameplayFeatures_PRICE);
		_Write_System_002EInt32_005B_005D(writer, value.gameplayFeatures_DEV_COSTS);
		_Write_System_002EInt32_005B_005D(writer, value.gameplayFeatures_DATE_YEAR);
		_Write_System_002EInt32_005B_005D(writer, value.gameplayFeatures_DATE_MONTH);
		_Write_System_002EInt32_005B_005D(writer, value.gameplayFeatures_GAMEPLAY);
		_Write_System_002EInt32_005B_005D(writer, value.gameplayFeatures_GRAPHIC);
		_Write_System_002EInt32_005B_005D(writer, value.gameplayFeatures_SOUND);
		_Write_System_002EInt32_005B_005D(writer, value.gameplayFeatures_TECHNIK);
		_Write_System_002EInt32_005B_005D(writer, value.gameplayFeatures_LEVEL);
		_Write_System_002EInt32_005B_005D(writer, value.gameplayFeatures_NEED_GAMEPLAY_FEATURE);
		_Write_System_002EBoolean_005B_005D(writer, value.gameplayFeatures_UNLOCK);
		_Write_System_002EBoolean_005B_005D(writer, value.gameplayFeatures_INTERNET);
		_Write_System_002EString_005B_005D(writer, value.gameplayFeatures_ICONFILE);
		_Write_System_002EBoolean_005B_005D(writer, value.gameplayFeatures_GOOD);
		_Write_System_002EBoolean_005B_005D(writer, value.gameplayFeatures_BAD);
		_Write_System_002EBoolean_005B_005D(writer, value.gameplayFeatures_LOCKPLATFORM);
		_Write_System_002EString_005B_005D(writer, value.gameplayFeatures_NAME_EN);
		_Write_System_002EString_005B_005D(writer, value.gameplayFeatures_NAME_GE);
		_Write_System_002EString_005B_005D(writer, value.gameplayFeatures_NAME_TU);
		_Write_System_002EString_005B_005D(writer, value.gameplayFeatures_NAME_CH);
		_Write_System_002EString_005B_005D(writer, value.gameplayFeatures_NAME_FR);
		_Write_System_002EString_005B_005D(writer, value.gameplayFeatures_NAME_PB);
		_Write_System_002EString_005B_005D(writer, value.gameplayFeatures_NAME_CT);
		_Write_System_002EString_005B_005D(writer, value.gameplayFeatures_NAME_HU);
		_Write_System_002EString_005B_005D(writer, value.gameplayFeatures_NAME_ES);
		_Write_System_002EString_005B_005D(writer, value.gameplayFeatures_NAME_CZ);
		_Write_System_002EString_005B_005D(writer, value.gameplayFeatures_NAME_KO);
		_Write_System_002EString_005B_005D(writer, value.gameplayFeatures_NAME_RU);
		_Write_System_002EString_005B_005D(writer, value.gameplayFeatures_NAME_IT);
		_Write_System_002EString_005B_005D(writer, value.gameplayFeatures_NAME_AR);
		_Write_System_002EString_005B_005D(writer, value.gameplayFeatures_NAME_JA);
		_Write_System_002EString_005B_005D(writer, value.gameplayFeatures_NAME_PL);
		_Write_System_002EString_005B_005D(writer, value.gameplayFeatures_NAME_UA);
		_Write_System_002EString_005B_005D(writer, value.gameplayFeatures_NAME_TH);
		_Write_System_002EString_005B_005D(writer, value.gameplayFeatures_DESC_EN);
		_Write_System_002EString_005B_005D(writer, value.gameplayFeatures_DESC_GE);
		_Write_System_002EString_005B_005D(writer, value.gameplayFeatures_DESC_TU);
		_Write_System_002EString_005B_005D(writer, value.gameplayFeatures_DESC_CH);
		_Write_System_002EString_005B_005D(writer, value.gameplayFeatures_DESC_FR);
		_Write_System_002EString_005B_005D(writer, value.gameplayFeatures_DESC_PB);
		_Write_System_002EString_005B_005D(writer, value.gameplayFeatures_DESC_CT);
		_Write_System_002EString_005B_005D(writer, value.gameplayFeatures_DESC_HU);
		_Write_System_002EString_005B_005D(writer, value.gameplayFeatures_DESC_ES);
		_Write_System_002EString_005B_005D(writer, value.gameplayFeatures_DESC_CZ);
		_Write_System_002EString_005B_005D(writer, value.gameplayFeatures_DESC_KO);
		_Write_System_002EString_005B_005D(writer, value.gameplayFeatures_DESC_RU);
		_Write_System_002EString_005B_005D(writer, value.gameplayFeatures_DESC_IT);
		_Write_System_002EString_005B_005D(writer, value.gameplayFeatures_DESC_AR);
		_Write_System_002EString_005B_005D(writer, value.gameplayFeatures_DESC_JA);
		_Write_System_002EString_005B_005D(writer, value.gameplayFeatures_DESC_PL);
		_Write_System_002EString_005B_005D(writer, value.gameplayFeatures_DESC_UA);
		_Write_System_002EString_005B_005D(writer, value.gameplayFeatures_DESC_TH);
	}

	public static mpCalls.s_EngineFeatures _Read_mpCalls_002Fs_EngineFeatures(NetworkReader reader)
	{
		return new mpCalls.s_EngineFeatures
		{
			engineFeatures_TYP = _Read_System_002EInt32_005B_005D(reader),
			engineFeatures_RES_POINTS = _Read_System_002EInt32_005B_005D(reader),
			engineFeatures_RES_POINTS_LEFT = _Read_System_002ESingle_005B_005D(reader),
			engineFeatures_PRICE = _Read_System_002EInt32_005B_005D(reader),
			engineFeatures_DEV_COSTS = _Read_System_002EInt32_005B_005D(reader),
			engineFeatures_TECH = _Read_System_002EInt32_005B_005D(reader),
			engineFeatures_DATE_YEAR = _Read_System_002EInt32_005B_005D(reader),
			engineFeatures_DATE_MONTH = _Read_System_002EInt32_005B_005D(reader),
			engineFeatures_GAMEPLAY = _Read_System_002EInt32_005B_005D(reader),
			engineFeatures_GRAPHIC = _Read_System_002EInt32_005B_005D(reader),
			engineFeatures_SOUND = _Read_System_002EInt32_005B_005D(reader),
			engineFeatures_TECHNIK = _Read_System_002EInt32_005B_005D(reader),
			engineFeatures_LEVEL = _Read_System_002EInt32_005B_005D(reader),
			engineFeatures_UNLOCK = _Read_System_002EBoolean_005B_005D(reader),
			engineFeatures_ICONFILE = _Read_System_002EString_005B_005D(reader),
			engineFeatures_NAME_EN = _Read_System_002EString_005B_005D(reader),
			engineFeatures_NAME_GE = _Read_System_002EString_005B_005D(reader),
			engineFeatures_NAME_TU = _Read_System_002EString_005B_005D(reader),
			engineFeatures_NAME_CH = _Read_System_002EString_005B_005D(reader),
			engineFeatures_NAME_FR = _Read_System_002EString_005B_005D(reader),
			engineFeatures_NAME_PB = _Read_System_002EString_005B_005D(reader),
			engineFeatures_NAME_CT = _Read_System_002EString_005B_005D(reader),
			engineFeatures_NAME_HU = _Read_System_002EString_005B_005D(reader),
			engineFeatures_NAME_ES = _Read_System_002EString_005B_005D(reader),
			engineFeatures_NAME_CZ = _Read_System_002EString_005B_005D(reader),
			engineFeatures_NAME_KO = _Read_System_002EString_005B_005D(reader),
			engineFeatures_NAME_AR = _Read_System_002EString_005B_005D(reader),
			engineFeatures_NAME_RU = _Read_System_002EString_005B_005D(reader),
			engineFeatures_NAME_IT = _Read_System_002EString_005B_005D(reader),
			engineFeatures_NAME_JA = _Read_System_002EString_005B_005D(reader),
			engineFeatures_NAME_PL = _Read_System_002EString_005B_005D(reader),
			engineFeatures_NAME_UA = _Read_System_002EString_005B_005D(reader),
			engineFeatures_NAME_TH = _Read_System_002EString_005B_005D(reader),
			engineFeatures_DESC_EN = _Read_System_002EString_005B_005D(reader),
			engineFeatures_DESC_GE = _Read_System_002EString_005B_005D(reader),
			engineFeatures_DESC_TU = _Read_System_002EString_005B_005D(reader),
			engineFeatures_DESC_CH = _Read_System_002EString_005B_005D(reader),
			engineFeatures_DESC_FR = _Read_System_002EString_005B_005D(reader),
			engineFeatures_DESC_PB = _Read_System_002EString_005B_005D(reader),
			engineFeatures_DESC_CT = _Read_System_002EString_005B_005D(reader),
			engineFeatures_DESC_HU = _Read_System_002EString_005B_005D(reader),
			engineFeatures_DESC_ES = _Read_System_002EString_005B_005D(reader),
			engineFeatures_DESC_CZ = _Read_System_002EString_005B_005D(reader),
			engineFeatures_DESC_KO = _Read_System_002EString_005B_005D(reader),
			engineFeatures_DESC_AR = _Read_System_002EString_005B_005D(reader),
			engineFeatures_DESC_RU = _Read_System_002EString_005B_005D(reader),
			engineFeatures_DESC_IT = _Read_System_002EString_005B_005D(reader),
			engineFeatures_DESC_JA = _Read_System_002EString_005B_005D(reader),
			engineFeatures_DESC_PL = _Read_System_002EString_005B_005D(reader),
			engineFeatures_DESC_UA = _Read_System_002EString_005B_005D(reader),
			engineFeatures_DESC_TH = _Read_System_002EString_005B_005D(reader)
		};
	}

	public static void _Write_mpCalls_002Fs_EngineFeatures(NetworkWriter writer, mpCalls.s_EngineFeatures value)
	{
		_Write_System_002EInt32_005B_005D(writer, value.engineFeatures_TYP);
		_Write_System_002EInt32_005B_005D(writer, value.engineFeatures_RES_POINTS);
		_Write_System_002ESingle_005B_005D(writer, value.engineFeatures_RES_POINTS_LEFT);
		_Write_System_002EInt32_005B_005D(writer, value.engineFeatures_PRICE);
		_Write_System_002EInt32_005B_005D(writer, value.engineFeatures_DEV_COSTS);
		_Write_System_002EInt32_005B_005D(writer, value.engineFeatures_TECH);
		_Write_System_002EInt32_005B_005D(writer, value.engineFeatures_DATE_YEAR);
		_Write_System_002EInt32_005B_005D(writer, value.engineFeatures_DATE_MONTH);
		_Write_System_002EInt32_005B_005D(writer, value.engineFeatures_GAMEPLAY);
		_Write_System_002EInt32_005B_005D(writer, value.engineFeatures_GRAPHIC);
		_Write_System_002EInt32_005B_005D(writer, value.engineFeatures_SOUND);
		_Write_System_002EInt32_005B_005D(writer, value.engineFeatures_TECHNIK);
		_Write_System_002EInt32_005B_005D(writer, value.engineFeatures_LEVEL);
		_Write_System_002EBoolean_005B_005D(writer, value.engineFeatures_UNLOCK);
		_Write_System_002EString_005B_005D(writer, value.engineFeatures_ICONFILE);
		_Write_System_002EString_005B_005D(writer, value.engineFeatures_NAME_EN);
		_Write_System_002EString_005B_005D(writer, value.engineFeatures_NAME_GE);
		_Write_System_002EString_005B_005D(writer, value.engineFeatures_NAME_TU);
		_Write_System_002EString_005B_005D(writer, value.engineFeatures_NAME_CH);
		_Write_System_002EString_005B_005D(writer, value.engineFeatures_NAME_FR);
		_Write_System_002EString_005B_005D(writer, value.engineFeatures_NAME_PB);
		_Write_System_002EString_005B_005D(writer, value.engineFeatures_NAME_CT);
		_Write_System_002EString_005B_005D(writer, value.engineFeatures_NAME_HU);
		_Write_System_002EString_005B_005D(writer, value.engineFeatures_NAME_ES);
		_Write_System_002EString_005B_005D(writer, value.engineFeatures_NAME_CZ);
		_Write_System_002EString_005B_005D(writer, value.engineFeatures_NAME_KO);
		_Write_System_002EString_005B_005D(writer, value.engineFeatures_NAME_AR);
		_Write_System_002EString_005B_005D(writer, value.engineFeatures_NAME_RU);
		_Write_System_002EString_005B_005D(writer, value.engineFeatures_NAME_IT);
		_Write_System_002EString_005B_005D(writer, value.engineFeatures_NAME_JA);
		_Write_System_002EString_005B_005D(writer, value.engineFeatures_NAME_PL);
		_Write_System_002EString_005B_005D(writer, value.engineFeatures_NAME_UA);
		_Write_System_002EString_005B_005D(writer, value.engineFeatures_NAME_TH);
		_Write_System_002EString_005B_005D(writer, value.engineFeatures_DESC_EN);
		_Write_System_002EString_005B_005D(writer, value.engineFeatures_DESC_GE);
		_Write_System_002EString_005B_005D(writer, value.engineFeatures_DESC_TU);
		_Write_System_002EString_005B_005D(writer, value.engineFeatures_DESC_CH);
		_Write_System_002EString_005B_005D(writer, value.engineFeatures_DESC_FR);
		_Write_System_002EString_005B_005D(writer, value.engineFeatures_DESC_PB);
		_Write_System_002EString_005B_005D(writer, value.engineFeatures_DESC_CT);
		_Write_System_002EString_005B_005D(writer, value.engineFeatures_DESC_HU);
		_Write_System_002EString_005B_005D(writer, value.engineFeatures_DESC_ES);
		_Write_System_002EString_005B_005D(writer, value.engineFeatures_DESC_CZ);
		_Write_System_002EString_005B_005D(writer, value.engineFeatures_DESC_KO);
		_Write_System_002EString_005B_005D(writer, value.engineFeatures_DESC_AR);
		_Write_System_002EString_005B_005D(writer, value.engineFeatures_DESC_RU);
		_Write_System_002EString_005B_005D(writer, value.engineFeatures_DESC_IT);
		_Write_System_002EString_005B_005D(writer, value.engineFeatures_DESC_JA);
		_Write_System_002EString_005B_005D(writer, value.engineFeatures_DESC_PL);
		_Write_System_002EString_005B_005D(writer, value.engineFeatures_DESC_UA);
		_Write_System_002EString_005B_005D(writer, value.engineFeatures_DESC_TH);
	}

	public static mpCalls.s_HardwareFeatures _Read_mpCalls_002Fs_HardwareFeatures(NetworkReader reader)
	{
		return new mpCalls.s_HardwareFeatures
		{
			hardFeat_ICONFILE = _Read_System_002EString_005B_005D(reader),
			hardFeat_RES_POINTS = _Read_System_002EInt32_005B_005D(reader),
			hardFeat_RES_POINTS_LEFT = _Read_System_002ESingle_005B_005D(reader),
			hardFeat_PRICE = _Read_System_002EInt32_005B_005D(reader),
			hardFeat_DEV_COSTS = _Read_System_002EInt32_005B_005D(reader),
			hardFeat_DATE_YEAR = _Read_System_002EInt32_005B_005D(reader),
			hardFeat_DATE_MONTH = _Read_System_002EInt32_005B_005D(reader),
			hardFeat_UNLOCK = _Read_System_002EBoolean_005B_005D(reader),
			hardFeat_ONLYSTATIONARY = _Read_System_002EBoolean_005B_005D(reader),
			hardFeat_ONLYHANDHELD = _Read_System_002EBoolean_005B_005D(reader),
			hardFeat_NEEDINTERNET = _Read_System_002EBoolean_005B_005D(reader),
			hardFeat_QUALITY = _Read_System_002ESingle_005B_005D(reader),
			hardFeat_NAME_EN = _Read_System_002EString_005B_005D(reader),
			hardFeat_NAME_GE = _Read_System_002EString_005B_005D(reader),
			hardFeat_NAME_TU = _Read_System_002EString_005B_005D(reader),
			hardFeat_NAME_CH = _Read_System_002EString_005B_005D(reader),
			hardFeat_NAME_FR = _Read_System_002EString_005B_005D(reader),
			hardFeat_NAME_PB = _Read_System_002EString_005B_005D(reader),
			hardFeat_NAME_CT = _Read_System_002EString_005B_005D(reader),
			hardFeat_NAME_HU = _Read_System_002EString_005B_005D(reader),
			hardFeat_NAME_ES = _Read_System_002EString_005B_005D(reader),
			hardFeat_NAME_CZ = _Read_System_002EString_005B_005D(reader),
			hardFeat_NAME_KO = _Read_System_002EString_005B_005D(reader),
			hardFeat_NAME_AR = _Read_System_002EString_005B_005D(reader),
			hardFeat_NAME_RU = _Read_System_002EString_005B_005D(reader),
			hardFeat_NAME_IT = _Read_System_002EString_005B_005D(reader),
			hardFeat_NAME_JA = _Read_System_002EString_005B_005D(reader),
			hardFeat_NAME_PL = _Read_System_002EString_005B_005D(reader),
			hardFeat_NAME_UA = _Read_System_002EString_005B_005D(reader),
			hardFeat_NAME_TH = _Read_System_002EString_005B_005D(reader),
			hardFeat_DESC_EN = _Read_System_002EString_005B_005D(reader),
			hardFeat_DESC_GE = _Read_System_002EString_005B_005D(reader),
			hardFeat_DESC_TU = _Read_System_002EString_005B_005D(reader),
			hardFeat_DESC_CH = _Read_System_002EString_005B_005D(reader),
			hardFeat_DESC_FR = _Read_System_002EString_005B_005D(reader),
			hardFeat_DESC_PB = _Read_System_002EString_005B_005D(reader),
			hardFeat_DESC_CT = _Read_System_002EString_005B_005D(reader),
			hardFeat_DESC_HU = _Read_System_002EString_005B_005D(reader),
			hardFeat_DESC_ES = _Read_System_002EString_005B_005D(reader),
			hardFeat_DESC_CZ = _Read_System_002EString_005B_005D(reader),
			hardFeat_DESC_KO = _Read_System_002EString_005B_005D(reader),
			hardFeat_DESC_AR = _Read_System_002EString_005B_005D(reader),
			hardFeat_DESC_RU = _Read_System_002EString_005B_005D(reader),
			hardFeat_DESC_IT = _Read_System_002EString_005B_005D(reader),
			hardFeat_DESC_JA = _Read_System_002EString_005B_005D(reader),
			hardFeat_DESC_PL = _Read_System_002EString_005B_005D(reader),
			hardFeat_DESC_UA = _Read_System_002EString_005B_005D(reader),
			hardFeat_DESC_TH = _Read_System_002EString_005B_005D(reader)
		};
	}

	public static void _Write_mpCalls_002Fs_HardwareFeatures(NetworkWriter writer, mpCalls.s_HardwareFeatures value)
	{
		_Write_System_002EString_005B_005D(writer, value.hardFeat_ICONFILE);
		_Write_System_002EInt32_005B_005D(writer, value.hardFeat_RES_POINTS);
		_Write_System_002ESingle_005B_005D(writer, value.hardFeat_RES_POINTS_LEFT);
		_Write_System_002EInt32_005B_005D(writer, value.hardFeat_PRICE);
		_Write_System_002EInt32_005B_005D(writer, value.hardFeat_DEV_COSTS);
		_Write_System_002EInt32_005B_005D(writer, value.hardFeat_DATE_YEAR);
		_Write_System_002EInt32_005B_005D(writer, value.hardFeat_DATE_MONTH);
		_Write_System_002EBoolean_005B_005D(writer, value.hardFeat_UNLOCK);
		_Write_System_002EBoolean_005B_005D(writer, value.hardFeat_ONLYSTATIONARY);
		_Write_System_002EBoolean_005B_005D(writer, value.hardFeat_ONLYHANDHELD);
		_Write_System_002EBoolean_005B_005D(writer, value.hardFeat_NEEDINTERNET);
		_Write_System_002ESingle_005B_005D(writer, value.hardFeat_QUALITY);
		_Write_System_002EString_005B_005D(writer, value.hardFeat_NAME_EN);
		_Write_System_002EString_005B_005D(writer, value.hardFeat_NAME_GE);
		_Write_System_002EString_005B_005D(writer, value.hardFeat_NAME_TU);
		_Write_System_002EString_005B_005D(writer, value.hardFeat_NAME_CH);
		_Write_System_002EString_005B_005D(writer, value.hardFeat_NAME_FR);
		_Write_System_002EString_005B_005D(writer, value.hardFeat_NAME_PB);
		_Write_System_002EString_005B_005D(writer, value.hardFeat_NAME_CT);
		_Write_System_002EString_005B_005D(writer, value.hardFeat_NAME_HU);
		_Write_System_002EString_005B_005D(writer, value.hardFeat_NAME_ES);
		_Write_System_002EString_005B_005D(writer, value.hardFeat_NAME_CZ);
		_Write_System_002EString_005B_005D(writer, value.hardFeat_NAME_KO);
		_Write_System_002EString_005B_005D(writer, value.hardFeat_NAME_AR);
		_Write_System_002EString_005B_005D(writer, value.hardFeat_NAME_RU);
		_Write_System_002EString_005B_005D(writer, value.hardFeat_NAME_IT);
		_Write_System_002EString_005B_005D(writer, value.hardFeat_NAME_JA);
		_Write_System_002EString_005B_005D(writer, value.hardFeat_NAME_PL);
		_Write_System_002EString_005B_005D(writer, value.hardFeat_NAME_UA);
		_Write_System_002EString_005B_005D(writer, value.hardFeat_NAME_TH);
		_Write_System_002EString_005B_005D(writer, value.hardFeat_DESC_EN);
		_Write_System_002EString_005B_005D(writer, value.hardFeat_DESC_GE);
		_Write_System_002EString_005B_005D(writer, value.hardFeat_DESC_TU);
		_Write_System_002EString_005B_005D(writer, value.hardFeat_DESC_CH);
		_Write_System_002EString_005B_005D(writer, value.hardFeat_DESC_FR);
		_Write_System_002EString_005B_005D(writer, value.hardFeat_DESC_PB);
		_Write_System_002EString_005B_005D(writer, value.hardFeat_DESC_CT);
		_Write_System_002EString_005B_005D(writer, value.hardFeat_DESC_HU);
		_Write_System_002EString_005B_005D(writer, value.hardFeat_DESC_ES);
		_Write_System_002EString_005B_005D(writer, value.hardFeat_DESC_CZ);
		_Write_System_002EString_005B_005D(writer, value.hardFeat_DESC_KO);
		_Write_System_002EString_005B_005D(writer, value.hardFeat_DESC_AR);
		_Write_System_002EString_005B_005D(writer, value.hardFeat_DESC_RU);
		_Write_System_002EString_005B_005D(writer, value.hardFeat_DESC_IT);
		_Write_System_002EString_005B_005D(writer, value.hardFeat_DESC_JA);
		_Write_System_002EString_005B_005D(writer, value.hardFeat_DESC_PL);
		_Write_System_002EString_005B_005D(writer, value.hardFeat_DESC_UA);
		_Write_System_002EString_005B_005D(writer, value.hardFeat_DESC_TH);
	}

	public static mpCalls.s_Hardware _Read_mpCalls_002Fs_Hardware(NetworkReader reader)
	{
		return new mpCalls.s_Hardware
		{
			hardware_ICONFILE = _Read_System_002EString_005B_005D(reader),
			hardware_TYP = _Read_System_002EInt32_005B_005D(reader),
			hardware_RES_POINTS = _Read_System_002EInt32_005B_005D(reader),
			hardware_RES_POINTS_LEFT = _Read_System_002ESingle_005B_005D(reader),
			hardware_PRICE = _Read_System_002EInt32_005B_005D(reader),
			hardware_DEV_COSTS = _Read_System_002EInt32_005B_005D(reader),
			hardware_TECH = _Read_System_002EInt32_005B_005D(reader),
			hardware_DATE_YEAR = _Read_System_002EInt32_005B_005D(reader),
			hardware_DATE_MONTH = _Read_System_002EInt32_005B_005D(reader),
			hardware_UNLOCK = _Read_System_002EBoolean_005B_005D(reader),
			hardware_ONLYSTATIONARY = _Read_System_002EBoolean_005B_005D(reader),
			hardware_ONLYHANDHELD = _Read_System_002EBoolean_005B_005D(reader),
			hardware_NEED1 = _Read_System_002EInt32_005B_005D(reader),
			hardware_NEED2 = _Read_System_002EInt32_005B_005D(reader),
			hardware_NAME_EN = _Read_System_002EString_005B_005D(reader),
			hardware_NAME_GE = _Read_System_002EString_005B_005D(reader),
			hardware_NAME_TU = _Read_System_002EString_005B_005D(reader),
			hardware_NAME_CH = _Read_System_002EString_005B_005D(reader),
			hardware_NAME_FR = _Read_System_002EString_005B_005D(reader),
			hardware_NAME_PB = _Read_System_002EString_005B_005D(reader),
			hardware_NAME_CT = _Read_System_002EString_005B_005D(reader),
			hardware_NAME_HU = _Read_System_002EString_005B_005D(reader),
			hardware_NAME_ES = _Read_System_002EString_005B_005D(reader),
			hardware_NAME_CZ = _Read_System_002EString_005B_005D(reader),
			hardware_NAME_KO = _Read_System_002EString_005B_005D(reader),
			hardware_NAME_AR = _Read_System_002EString_005B_005D(reader),
			hardware_NAME_RU = _Read_System_002EString_005B_005D(reader),
			hardware_NAME_IT = _Read_System_002EString_005B_005D(reader),
			hardware_NAME_JA = _Read_System_002EString_005B_005D(reader),
			hardware_NAME_PL = _Read_System_002EString_005B_005D(reader),
			hardware_NAME_UA = _Read_System_002EString_005B_005D(reader),
			hardware_NAME_TH = _Read_System_002EString_005B_005D(reader),
			hardware_DESC_EN = _Read_System_002EString_005B_005D(reader),
			hardware_DESC_GE = _Read_System_002EString_005B_005D(reader),
			hardware_DESC_TU = _Read_System_002EString_005B_005D(reader),
			hardware_DESC_CH = _Read_System_002EString_005B_005D(reader),
			hardware_DESC_FR = _Read_System_002EString_005B_005D(reader),
			hardware_DESC_PB = _Read_System_002EString_005B_005D(reader),
			hardware_DESC_CT = _Read_System_002EString_005B_005D(reader),
			hardware_DESC_HU = _Read_System_002EString_005B_005D(reader),
			hardware_DESC_ES = _Read_System_002EString_005B_005D(reader),
			hardware_DESC_CZ = _Read_System_002EString_005B_005D(reader),
			hardware_DESC_KO = _Read_System_002EString_005B_005D(reader),
			hardware_DESC_AR = _Read_System_002EString_005B_005D(reader),
			hardware_DESC_RU = _Read_System_002EString_005B_005D(reader),
			hardware_DESC_IT = _Read_System_002EString_005B_005D(reader),
			hardware_DESC_JA = _Read_System_002EString_005B_005D(reader),
			hardware_DESC_PL = _Read_System_002EString_005B_005D(reader),
			hardware_DESC_UA = _Read_System_002EString_005B_005D(reader),
			hardware_DESC_TH = _Read_System_002EString_005B_005D(reader)
		};
	}

	public static void _Write_mpCalls_002Fs_Hardware(NetworkWriter writer, mpCalls.s_Hardware value)
	{
		_Write_System_002EString_005B_005D(writer, value.hardware_ICONFILE);
		_Write_System_002EInt32_005B_005D(writer, value.hardware_TYP);
		_Write_System_002EInt32_005B_005D(writer, value.hardware_RES_POINTS);
		_Write_System_002ESingle_005B_005D(writer, value.hardware_RES_POINTS_LEFT);
		_Write_System_002EInt32_005B_005D(writer, value.hardware_PRICE);
		_Write_System_002EInt32_005B_005D(writer, value.hardware_DEV_COSTS);
		_Write_System_002EInt32_005B_005D(writer, value.hardware_TECH);
		_Write_System_002EInt32_005B_005D(writer, value.hardware_DATE_YEAR);
		_Write_System_002EInt32_005B_005D(writer, value.hardware_DATE_MONTH);
		_Write_System_002EBoolean_005B_005D(writer, value.hardware_UNLOCK);
		_Write_System_002EBoolean_005B_005D(writer, value.hardware_ONLYSTATIONARY);
		_Write_System_002EBoolean_005B_005D(writer, value.hardware_ONLYHANDHELD);
		_Write_System_002EInt32_005B_005D(writer, value.hardware_NEED1);
		_Write_System_002EInt32_005B_005D(writer, value.hardware_NEED2);
		_Write_System_002EString_005B_005D(writer, value.hardware_NAME_EN);
		_Write_System_002EString_005B_005D(writer, value.hardware_NAME_GE);
		_Write_System_002EString_005B_005D(writer, value.hardware_NAME_TU);
		_Write_System_002EString_005B_005D(writer, value.hardware_NAME_CH);
		_Write_System_002EString_005B_005D(writer, value.hardware_NAME_FR);
		_Write_System_002EString_005B_005D(writer, value.hardware_NAME_PB);
		_Write_System_002EString_005B_005D(writer, value.hardware_NAME_CT);
		_Write_System_002EString_005B_005D(writer, value.hardware_NAME_HU);
		_Write_System_002EString_005B_005D(writer, value.hardware_NAME_ES);
		_Write_System_002EString_005B_005D(writer, value.hardware_NAME_CZ);
		_Write_System_002EString_005B_005D(writer, value.hardware_NAME_KO);
		_Write_System_002EString_005B_005D(writer, value.hardware_NAME_AR);
		_Write_System_002EString_005B_005D(writer, value.hardware_NAME_RU);
		_Write_System_002EString_005B_005D(writer, value.hardware_NAME_IT);
		_Write_System_002EString_005B_005D(writer, value.hardware_NAME_JA);
		_Write_System_002EString_005B_005D(writer, value.hardware_NAME_PL);
		_Write_System_002EString_005B_005D(writer, value.hardware_NAME_UA);
		_Write_System_002EString_005B_005D(writer, value.hardware_NAME_TH);
		_Write_System_002EString_005B_005D(writer, value.hardware_DESC_EN);
		_Write_System_002EString_005B_005D(writer, value.hardware_DESC_GE);
		_Write_System_002EString_005B_005D(writer, value.hardware_DESC_TU);
		_Write_System_002EString_005B_005D(writer, value.hardware_DESC_CH);
		_Write_System_002EString_005B_005D(writer, value.hardware_DESC_FR);
		_Write_System_002EString_005B_005D(writer, value.hardware_DESC_PB);
		_Write_System_002EString_005B_005D(writer, value.hardware_DESC_CT);
		_Write_System_002EString_005B_005D(writer, value.hardware_DESC_HU);
		_Write_System_002EString_005B_005D(writer, value.hardware_DESC_ES);
		_Write_System_002EString_005B_005D(writer, value.hardware_DESC_CZ);
		_Write_System_002EString_005B_005D(writer, value.hardware_DESC_KO);
		_Write_System_002EString_005B_005D(writer, value.hardware_DESC_AR);
		_Write_System_002EString_005B_005D(writer, value.hardware_DESC_RU);
		_Write_System_002EString_005B_005D(writer, value.hardware_DESC_IT);
		_Write_System_002EString_005B_005D(writer, value.hardware_DESC_JA);
		_Write_System_002EString_005B_005D(writer, value.hardware_DESC_PL);
		_Write_System_002EString_005B_005D(writer, value.hardware_DESC_UA);
		_Write_System_002EString_005B_005D(writer, value.hardware_DESC_TH);
	}

	public static mpCalls.s_AntiCheat _Read_mpCalls_002Fs_AntiCheat(NetworkReader reader)
	{
		return new mpCalls.s_AntiCheat
		{
			myID = reader.ReadInt(),
			date_year = reader.ReadInt(),
			date_month = reader.ReadInt(),
			price = reader.ReadInt(),
			dev_costs = reader.ReadInt(),
			name_EN = reader.ReadString(),
			name_GE = reader.ReadString(),
			name_TU = reader.ReadString(),
			name_CH = reader.ReadString(),
			name_FR = reader.ReadString(),
			name_CT = reader.ReadString(),
			name_RU = reader.ReadString(),
			name_IT = reader.ReadString(),
			name_JA = reader.ReadString(),
			name_UA = reader.ReadString(),
			name_TH = reader.ReadString(),
			isUnlocked = reader.ReadBool(),
			effekt = reader.ReadFloat(),
			neverLooseEffect = reader.ReadBool()
		};
	}

	public static void _Write_mpCalls_002Fs_AntiCheat(NetworkWriter writer, mpCalls.s_AntiCheat value)
	{
		writer.WriteInt(value.myID);
		writer.WriteInt(value.date_year);
		writer.WriteInt(value.date_month);
		writer.WriteInt(value.price);
		writer.WriteInt(value.dev_costs);
		writer.WriteString(value.name_EN);
		writer.WriteString(value.name_GE);
		writer.WriteString(value.name_TU);
		writer.WriteString(value.name_CH);
		writer.WriteString(value.name_FR);
		writer.WriteString(value.name_CT);
		writer.WriteString(value.name_RU);
		writer.WriteString(value.name_IT);
		writer.WriteString(value.name_JA);
		writer.WriteString(value.name_UA);
		writer.WriteString(value.name_TH);
		writer.WriteBool(value.isUnlocked);
		writer.WriteFloat(value.effekt);
		writer.WriteBool(value.neverLooseEffect);
	}

	public static mpCalls.s_CopyProtect _Read_mpCalls_002Fs_CopyProtect(NetworkReader reader)
	{
		return new mpCalls.s_CopyProtect
		{
			myID = reader.ReadInt(),
			date_year = reader.ReadInt(),
			date_month = reader.ReadInt(),
			price = reader.ReadInt(),
			dev_costs = reader.ReadInt(),
			name_EN = reader.ReadString(),
			name_GE = reader.ReadString(),
			name_TU = reader.ReadString(),
			name_CH = reader.ReadString(),
			name_FR = reader.ReadString(),
			name_CT = reader.ReadString(),
			name_RU = reader.ReadString(),
			name_IT = reader.ReadString(),
			name_JA = reader.ReadString(),
			name_UA = reader.ReadString(),
			name_TH = reader.ReadString(),
			isUnlocked = reader.ReadBool(),
			effekt = reader.ReadFloat(),
			neverLooseEffect = reader.ReadBool()
		};
	}

	public static void _Write_mpCalls_002Fs_CopyProtect(NetworkWriter writer, mpCalls.s_CopyProtect value)
	{
		writer.WriteInt(value.myID);
		writer.WriteInt(value.date_year);
		writer.WriteInt(value.date_month);
		writer.WriteInt(value.price);
		writer.WriteInt(value.dev_costs);
		writer.WriteString(value.name_EN);
		writer.WriteString(value.name_GE);
		writer.WriteString(value.name_TU);
		writer.WriteString(value.name_CH);
		writer.WriteString(value.name_FR);
		writer.WriteString(value.name_CT);
		writer.WriteString(value.name_RU);
		writer.WriteString(value.name_IT);
		writer.WriteString(value.name_JA);
		writer.WriteString(value.name_UA);
		writer.WriteString(value.name_TH);
		writer.WriteBool(value.isUnlocked);
		writer.WriteFloat(value.effekt);
		writer.WriteBool(value.neverLooseEffect);
	}

	public static mpCalls.s_NpcEngine _Read_mpCalls_002Fs_NpcEngine(NetworkReader reader)
	{
		return new mpCalls.s_NpcEngine
		{
			myID = reader.ReadInt(),
			ownerID = reader.ReadInt(),
			isUnlocked = reader.ReadBool(),
			gekauft = reader.ReadBool(),
			myName = reader.ReadString(),
			umsatz = reader.ReadInt(),
			name_EN = reader.ReadString(),
			name_GE = reader.ReadString(),
			name_TU = reader.ReadString(),
			name_CH = reader.ReadString(),
			name_FR = reader.ReadString(),
			name_HU = reader.ReadString(),
			name_CT = reader.ReadString(),
			name_CZ = reader.ReadString(),
			name_PB = reader.ReadString(),
			name_IT = reader.ReadString(),
			name_JA = reader.ReadString(),
			name_UA = reader.ReadString(),
			name_PL = reader.ReadString(),
			name_TH = reader.ReadString(),
			features = _Read_System_002EBoolean_005B_005D(reader),
			featuresInDev = _Read_System_002EBoolean_005B_005D(reader),
			spezialgenre = reader.ReadInt(),
			spezialplatform = reader.ReadInt(),
			sellEngine = reader.ReadBool(),
			preis = reader.ReadInt(),
			gewinnbeteiligung = reader.ReadInt(),
			updating = reader.ReadBool(),
			devPoints = reader.ReadFloat(),
			devPointsStart = reader.ReadFloat(),
			date_year = reader.ReadInt(),
			date_month = reader.ReadInt(),
			publisherBuyed = _Read_System_002EBoolean_005B_005D(reader),
			archiv_engine = reader.ReadBool()
		};
	}

	public static void _Write_mpCalls_002Fs_NpcEngine(NetworkWriter writer, mpCalls.s_NpcEngine value)
	{
		writer.WriteInt(value.myID);
		writer.WriteInt(value.ownerID);
		writer.WriteBool(value.isUnlocked);
		writer.WriteBool(value.gekauft);
		writer.WriteString(value.myName);
		writer.WriteInt(value.umsatz);
		writer.WriteString(value.name_EN);
		writer.WriteString(value.name_GE);
		writer.WriteString(value.name_TU);
		writer.WriteString(value.name_CH);
		writer.WriteString(value.name_FR);
		writer.WriteString(value.name_HU);
		writer.WriteString(value.name_CT);
		writer.WriteString(value.name_CZ);
		writer.WriteString(value.name_PB);
		writer.WriteString(value.name_IT);
		writer.WriteString(value.name_JA);
		writer.WriteString(value.name_UA);
		writer.WriteString(value.name_PL);
		writer.WriteString(value.name_TH);
		_Write_System_002EBoolean_005B_005D(writer, value.features);
		_Write_System_002EBoolean_005B_005D(writer, value.featuresInDev);
		writer.WriteInt(value.spezialgenre);
		writer.WriteInt(value.spezialplatform);
		writer.WriteBool(value.sellEngine);
		writer.WriteInt(value.preis);
		writer.WriteInt(value.gewinnbeteiligung);
		writer.WriteBool(value.updating);
		writer.WriteFloat(value.devPoints);
		writer.WriteFloat(value.devPointsStart);
		writer.WriteInt(value.date_year);
		writer.WriteInt(value.date_month);
		_Write_System_002EBoolean_005B_005D(writer, value.publisherBuyed);
		writer.WriteBool(value.archiv_engine);
	}

	public static mpCalls.s_TochterfirmaUmsatz _Read_mpCalls_002Fs_TochterfirmaUmsatz(NetworkReader reader)
	{
		return new mpCalls.s_TochterfirmaUmsatz
		{
			publisherID = reader.ReadInt(),
			gameID = reader.ReadInt(),
			money = reader.ReadLong()
		};
	}

	public static void _Write_mpCalls_002Fs_TochterfirmaUmsatz(NetworkWriter writer, mpCalls.s_TochterfirmaUmsatz value)
	{
		writer.WriteInt(value.publisherID);
		writer.WriteInt(value.gameID);
		writer.WriteLong(value.money);
	}

	public static mpCalls.s_Firmenwert _Read_mpCalls_002Fs_Firmenwert(NetworkReader reader)
	{
		return new mpCalls.s_Firmenwert
		{
			publisherID = _Read_System_002EInt32_005B_005D(reader),
			firmenwert = _Read_System_002EInt64_005B_005D(reader)
		};
	}

	public static long[] _Read_System_002EInt64_005B_005D(NetworkReader reader)
	{
		return reader.ReadArray<long>();
	}

	public static void _Write_mpCalls_002Fs_Firmenwert(NetworkWriter writer, mpCalls.s_Firmenwert value)
	{
		_Write_System_002EInt32_005B_005D(writer, value.publisherID);
		_Write_System_002EInt64_005B_005D(writer, value.firmenwert);
	}

	public static void _Write_System_002EInt64_005B_005D(NetworkWriter writer, long[] value)
	{
		writer.WriteArray(value);
	}

	public static mpCalls.s_NpcGameName _Read_mpCalls_002Fs_NpcGameName(NetworkReader reader)
	{
		return new mpCalls.s_NpcGameName
		{
			slot = reader.ReadInt(),
			ip = reader.ReadBool()
		};
	}

	public static void _Write_mpCalls_002Fs_NpcGameName(NetworkWriter writer, mpCalls.s_NpcGameName value)
	{
		writer.WriteInt(value.slot);
		writer.WriteBool(value.ip);
	}

	public static mpCalls.s_BlockContractGame _Read_mpCalls_002Fs_BlockContractGame(NetworkReader reader)
	{
		return new mpCalls.s_BlockContractGame
		{
			myID = reader.ReadInt(),
			block = reader.ReadBool()
		};
	}

	public static void _Write_mpCalls_002Fs_BlockContractGame(NetworkWriter writer, mpCalls.s_BlockContractGame value)
	{
		writer.WriteInt(value.myID);
		writer.WriteBool(value.block);
	}

	public static mpCalls.s_Publisher _Read_mpCalls_002Fs_Publisher(NetworkReader reader)
	{
		return new mpCalls.s_Publisher
		{
			myID = reader.ReadInt(),
			isUnlocked = reader.ReadBool(),
			name_EN = reader.ReadString(),
			name_GE = reader.ReadString(),
			name_TU = reader.ReadString(),
			name_CH = reader.ReadString(),
			name_FR = reader.ReadString(),
			name_JA = reader.ReadString(),
			name_UA = reader.ReadString(),
			name_TH = reader.ReadString(),
			date_year = reader.ReadInt(),
			date_month = reader.ReadInt(),
			stars = reader.ReadFloat(),
			logoID = reader.ReadInt(),
			developer = reader.ReadBool(),
			publisher = reader.ReadBool(),
			onlyMobile = reader.ReadBool(),
			share = reader.ReadFloat(),
			fanGenre = reader.ReadInt(),
			firmenwert = reader.ReadLong(),
			notForSale = reader.ReadBool(),
			lockToBuy = reader.ReadInt(),
			isPlayer = reader.ReadBool(),
			ownerID = reader.ReadInt(),
			country = reader.ReadInt(),
			ownPlatform = reader.ReadBool(),
			exklusive = reader.ReadBool(),
			awards = _Read_System_002EInt32_005B_005D(reader)
		};
	}

	public static void _Write_mpCalls_002Fs_Publisher(NetworkWriter writer, mpCalls.s_Publisher value)
	{
		writer.WriteInt(value.myID);
		writer.WriteBool(value.isUnlocked);
		writer.WriteString(value.name_EN);
		writer.WriteString(value.name_GE);
		writer.WriteString(value.name_TU);
		writer.WriteString(value.name_CH);
		writer.WriteString(value.name_FR);
		writer.WriteString(value.name_JA);
		writer.WriteString(value.name_UA);
		writer.WriteString(value.name_TH);
		writer.WriteInt(value.date_year);
		writer.WriteInt(value.date_month);
		writer.WriteFloat(value.stars);
		writer.WriteInt(value.logoID);
		writer.WriteBool(value.developer);
		writer.WriteBool(value.publisher);
		writer.WriteBool(value.onlyMobile);
		writer.WriteFloat(value.share);
		writer.WriteInt(value.fanGenre);
		writer.WriteLong(value.firmenwert);
		writer.WriteBool(value.notForSale);
		writer.WriteInt(value.lockToBuy);
		writer.WriteBool(value.isPlayer);
		writer.WriteInt(value.ownerID);
		writer.WriteInt(value.country);
		writer.WriteBool(value.ownPlatform);
		writer.WriteBool(value.exklusive);
		_Write_System_002EInt32_005B_005D(writer, value.awards);
	}

	public static mpCalls.s_PublisherOwner _Read_mpCalls_002Fs_PublisherOwner(NetworkReader reader)
	{
		return new mpCalls.s_PublisherOwner
		{
			myID = reader.ReadInt(),
			ownerID = reader.ReadInt()
		};
	}

	public static void _Write_mpCalls_002Fs_PublisherOwner(NetworkWriter writer, mpCalls.s_PublisherOwner value)
	{
		writer.WriteInt(value.myID);
		writer.WriteInt(value.ownerID);
	}

	public static mpCalls.s_PublisherClose _Read_mpCalls_002Fs_PublisherClose(NetworkReader reader)
	{
		return new mpCalls.s_PublisherClose
		{
			myID = reader.ReadInt()
		};
	}

	public static void _Write_mpCalls_002Fs_PublisherClose(NetworkWriter writer, mpCalls.s_PublisherClose value)
	{
		writer.WriteInt(value.myID);
	}

	public static mpCalls.s_PublisherTochterfirmaSettings _Read_mpCalls_002Fs_PublisherTochterfirmaSettings(NetworkReader reader)
	{
		return new mpCalls.s_PublisherTochterfirmaSettings
		{
			myID = reader.ReadInt(),
			tf_geschlossen = reader.ReadBool(),
			tf_autoRelease = reader.ReadBool(),
			tf_onlyPlayerConsole = reader.ReadBool(),
			tf_allowMMO = reader.ReadBool(),
			tf_allowF2P = reader.ReadBool(),
			tf_allowAddon = reader.ReadBool(),
			tf_noArcade = reader.ReadBool(),
			tf_noHandy = reader.ReadBool(),
			tf_noRetro = reader.ReadBool(),
			tf_noPorts = reader.ReadBool(),
			tf_noBudget = reader.ReadBool(),
			tf_noGOTY = reader.ReadBool(),
			tf_noBundles = reader.ReadBool(),
			tf_noAddonBundles = reader.ReadBool(),
			tf_noRemaster = reader.ReadBool(),
			tf_noSpinoffs = reader.ReadBool(),
			tf_autoGamePass = reader.ReadBool(),
			tf_ownPublisher = reader.ReadBool(),
			tf_publisher = reader.ReadBool(),
			tf_developer = reader.ReadBool(),
			tf_entwicklungsdauer = reader.ReadInt(),
			tf_gameSize = reader.ReadInt(),
			tf_gameGenre = reader.ReadInt(),
			tf_gameTopic = reader.ReadInt(),
			tf_engine = reader.ReadInt(),
			tf_autoReleaseVal = reader.ReadInt(),
			tf_ownPublisherPriority = reader.ReadInt(),
			tf_ipFocus = _Read_System_002EInt32_005B_005D(reader),
			tf_platformFocus = _Read_System_002EInt32_005B_005D(reader)
		};
	}

	public static void _Write_mpCalls_002Fs_PublisherTochterfirmaSettings(NetworkWriter writer, mpCalls.s_PublisherTochterfirmaSettings value)
	{
		writer.WriteInt(value.myID);
		writer.WriteBool(value.tf_geschlossen);
		writer.WriteBool(value.tf_autoRelease);
		writer.WriteBool(value.tf_onlyPlayerConsole);
		writer.WriteBool(value.tf_allowMMO);
		writer.WriteBool(value.tf_allowF2P);
		writer.WriteBool(value.tf_allowAddon);
		writer.WriteBool(value.tf_noArcade);
		writer.WriteBool(value.tf_noHandy);
		writer.WriteBool(value.tf_noRetro);
		writer.WriteBool(value.tf_noPorts);
		writer.WriteBool(value.tf_noBudget);
		writer.WriteBool(value.tf_noGOTY);
		writer.WriteBool(value.tf_noBundles);
		writer.WriteBool(value.tf_noAddonBundles);
		writer.WriteBool(value.tf_noRemaster);
		writer.WriteBool(value.tf_noSpinoffs);
		writer.WriteBool(value.tf_autoGamePass);
		writer.WriteBool(value.tf_ownPublisher);
		writer.WriteBool(value.tf_publisher);
		writer.WriteBool(value.tf_developer);
		writer.WriteInt(value.tf_entwicklungsdauer);
		writer.WriteInt(value.tf_gameSize);
		writer.WriteInt(value.tf_gameGenre);
		writer.WriteInt(value.tf_gameTopic);
		writer.WriteInt(value.tf_engine);
		writer.WriteInt(value.tf_autoReleaseVal);
		writer.WriteInt(value.tf_ownPublisherPriority);
		_Write_System_002EInt32_005B_005D(writer, value.tf_ipFocus);
		_Write_System_002EInt32_005B_005D(writer, value.tf_platformFocus);
	}

	public static mpCalls.s_exklusivKonsolenSells _Read_mpCalls_002Fs_exklusivKonsolenSells(NetworkReader reader)
	{
		return new mpCalls.s_exklusivKonsolenSells
		{
			gameID = reader.ReadInt(),
			exklusivKonsolenSells = reader.ReadLong()
		};
	}

	public static void _Write_mpCalls_002Fs_exklusivKonsolenSells(NetworkWriter writer, mpCalls.s_exklusivKonsolenSells value)
	{
		writer.WriteInt(value.gameID);
		writer.WriteLong(value.exklusivKonsolenSells);
	}

	public static mpCalls.s_GameAnkuendigung _Read_mpCalls_002Fs_GameAnkuendigung(NetworkReader reader)
	{
		return new mpCalls.s_GameAnkuendigung
		{
			gameID = reader.ReadInt()
		};
	}

	public static void _Write_mpCalls_002Fs_GameAnkuendigung(NetworkWriter writer, mpCalls.s_GameAnkuendigung value)
	{
		writer.WriteInt(value.gameID);
	}

	public static mpCalls.s_GameDestroy _Read_mpCalls_002Fs_GameDestroy(NetworkReader reader)
	{
		return new mpCalls.s_GameDestroy
		{
			gameID = reader.ReadInt()
		};
	}

	public static void _Write_mpCalls_002Fs_GameDestroy(NetworkWriter writer, mpCalls.s_GameDestroy value)
	{
		writer.WriteInt(value.gameID);
	}

	public static mpCalls.s_GameRemoveFromMarket _Read_mpCalls_002Fs_GameRemoveFromMarket(NetworkReader reader)
	{
		return new mpCalls.s_GameRemoveFromMarket
		{
			gameID = reader.ReadInt()
		};
	}

	public static void _Write_mpCalls_002Fs_GameRemoveFromMarket(NetworkWriter writer, mpCalls.s_GameRemoveFromMarket value)
	{
		writer.WriteInt(value.gameID);
	}

	public static mpCalls.s_GameOwner _Read_mpCalls_002Fs_GameOwner(NetworkReader reader)
	{
		return new mpCalls.s_GameOwner
		{
			gameID = reader.ReadInt(),
			ownerID = reader.ReadInt()
		};
	}

	public static void _Write_mpCalls_002Fs_GameOwner(NetworkWriter writer, mpCalls.s_GameOwner value)
	{
		writer.WriteInt(value.gameID);
		writer.WriteInt(value.ownerID);
	}

	public static mpCalls.s_GameIpSell _Read_mpCalls_002Fs_GameIpSell(NetworkReader reader)
	{
		return new mpCalls.s_GameIpSell
		{
			gameID = reader.ReadInt(),
			ipToSell = reader.ReadBool()
		};
	}

	public static void _Write_mpCalls_002Fs_GameIpSell(NetworkWriter writer, mpCalls.s_GameIpSell value)
	{
		writer.WriteInt(value.gameID);
		writer.WriteBool(value.ipToSell);
	}

	public static mpCalls.s_GameIpPoints _Read_mpCalls_002Fs_GameIpPoints(NetworkReader reader)
	{
		return new mpCalls.s_GameIpPoints
		{
			gameID = reader.ReadInt(),
			ipPunkte = reader.ReadFloat(),
			ipTime = reader.ReadInt()
		};
	}

	public static void _Write_mpCalls_002Fs_GameIpPoints(NetworkWriter writer, mpCalls.s_GameIpPoints value)
	{
		writer.WriteInt(value.gameID);
		writer.WriteFloat(value.ipPunkte);
		writer.WriteInt(value.ipTime);
	}

	public static mpCalls.s_GameSell _Read_mpCalls_002Fs_GameSell(NetworkReader reader)
	{
		return new mpCalls.s_GameSell
		{
			gameID = reader.ReadInt(),
			isOnMarket = reader.ReadBool(),
			weeksOnMarket = reader.ReadInt(),
			sellsTotal = reader.ReadLong(),
			sellsTotalStandard = reader.ReadLong(),
			sellsTotalDeluxe = reader.ReadLong(),
			sellsTotalCollectors = reader.ReadLong(),
			sellsTotalOnline = reader.ReadLong(),
			abonnements = reader.ReadLong(),
			abonnementsWoche = reader.ReadLong(),
			bestAbonnements = reader.ReadLong(),
			exklusivKonsolenSells = reader.ReadLong(),
			userPositiv = reader.ReadInt(),
			userNegativ = reader.ReadInt(),
			costs_entwicklung = reader.ReadLong(),
			costs_mitarbeiter = reader.ReadLong(),
			costs_marketing = reader.ReadLong(),
			costs_enginegebuehren = reader.ReadLong(),
			costs_server = reader.ReadLong(),
			costs_production = reader.ReadLong(),
			costs_updates = reader.ReadLong(),
			points_gameplay = reader.ReadFloat(),
			points_grafik = reader.ReadFloat(),
			points_sound = reader.ReadFloat(),
			points_technik = reader.ReadFloat(),
			points_bugs = reader.ReadFloat(),
			points_bugsInvis = reader.ReadFloat(),
			umsatzTotal = reader.ReadLong(),
			umsatzInApp = reader.ReadLong(),
			umsatzAbos = reader.ReadLong(),
			bestChartPosition = reader.ReadInt(),
			lastChartPosition = reader.ReadInt(),
			f2pInteresse = reader.ReadFloat(),
			mmoInteresse = reader.ReadFloat(),
			vorbestellungen = reader.ReadInt(),
			realsticPower = reader.ReadFloat(),
			hype = reader.ReadFloat(),
			stornierungen = reader.ReadInt(),
			commercialFlop = reader.ReadBool(),
			commercialHit = reader.ReadBool(),
			freigabeBudget = reader.ReadInt(),
			releaseDate = reader.ReadInt(),
			inAppPurchaseWeek = reader.ReadInt(),
			sellsPerWeek = _Read_System_002EInt32_005B_005D(reader),
			verkaufspreis = _Read_System_002EInt32_005B_005D(reader)
		};
	}

	public static void _Write_mpCalls_002Fs_GameSell(NetworkWriter writer, mpCalls.s_GameSell value)
	{
		writer.WriteInt(value.gameID);
		writer.WriteBool(value.isOnMarket);
		writer.WriteInt(value.weeksOnMarket);
		writer.WriteLong(value.sellsTotal);
		writer.WriteLong(value.sellsTotalStandard);
		writer.WriteLong(value.sellsTotalDeluxe);
		writer.WriteLong(value.sellsTotalCollectors);
		writer.WriteLong(value.sellsTotalOnline);
		writer.WriteLong(value.abonnements);
		writer.WriteLong(value.abonnementsWoche);
		writer.WriteLong(value.bestAbonnements);
		writer.WriteLong(value.exklusivKonsolenSells);
		writer.WriteInt(value.userPositiv);
		writer.WriteInt(value.userNegativ);
		writer.WriteLong(value.costs_entwicklung);
		writer.WriteLong(value.costs_mitarbeiter);
		writer.WriteLong(value.costs_marketing);
		writer.WriteLong(value.costs_enginegebuehren);
		writer.WriteLong(value.costs_server);
		writer.WriteLong(value.costs_production);
		writer.WriteLong(value.costs_updates);
		writer.WriteFloat(value.points_gameplay);
		writer.WriteFloat(value.points_grafik);
		writer.WriteFloat(value.points_sound);
		writer.WriteFloat(value.points_technik);
		writer.WriteFloat(value.points_bugs);
		writer.WriteFloat(value.points_bugsInvis);
		writer.WriteLong(value.umsatzTotal);
		writer.WriteLong(value.umsatzInApp);
		writer.WriteLong(value.umsatzAbos);
		writer.WriteInt(value.bestChartPosition);
		writer.WriteInt(value.lastChartPosition);
		writer.WriteFloat(value.f2pInteresse);
		writer.WriteFloat(value.mmoInteresse);
		writer.WriteInt(value.vorbestellungen);
		writer.WriteFloat(value.realsticPower);
		writer.WriteFloat(value.hype);
		writer.WriteInt(value.stornierungen);
		writer.WriteBool(value.commercialFlop);
		writer.WriteBool(value.commercialHit);
		writer.WriteInt(value.freigabeBudget);
		writer.WriteInt(value.releaseDate);
		writer.WriteInt(value.inAppPurchaseWeek);
		_Write_System_002EInt32_005B_005D(writer, value.sellsPerWeek);
		_Write_System_002EInt32_005B_005D(writer, value.verkaufspreis);
	}

	public static mpCalls.s_Game _Read_mpCalls_002Fs_Game(NetworkReader reader)
	{
		return new mpCalls.s_Game
		{
			gameID = reader.ReadInt(),
			myName = reader.ReadString(),
			ipName = reader.ReadString(),
			playerGame = reader.ReadBool(),
			inDevelopment = reader.ReadBool(),
			developerID = reader.ReadInt(),
			publisherID = reader.ReadInt(),
			ownerID = reader.ReadInt(),
			engineID = reader.ReadInt(),
			hype = reader.ReadFloat(),
			isOnMarket = reader.ReadBool(),
			warBeiAwards = reader.ReadBool(),
			weeksOnMarket = reader.ReadInt(),
			usk = reader.ReadInt(),
			freigabeBudget = reader.ReadInt(),
			reviewGameplay = reader.ReadInt(),
			reviewGrafik = reader.ReadInt(),
			reviewSound = reader.ReadInt(),
			reviewSteuerung = reader.ReadInt(),
			reviewTotal = reader.ReadInt(),
			reviewGameplayText = reader.ReadInt(),
			reviewGrafikText = reader.ReadInt(),
			reviewSoundText = reader.ReadInt(),
			reviewSteuerungText = reader.ReadInt(),
			reviewTotalText = reader.ReadInt(),
			date_year = reader.ReadInt(),
			date_month = reader.ReadInt(),
			date_start_year = reader.ReadInt(),
			date_start_month = reader.ReadInt(),
			sellsTotal = reader.ReadLong(),
			umsatzTotal = reader.ReadLong(),
			costs_entwicklung = reader.ReadLong(),
			costs_mitarbeiter = reader.ReadLong(),
			costs_marketing = reader.ReadLong(),
			costs_enginegebuehren = reader.ReadLong(),
			costs_server = reader.ReadLong(),
			costs_production = reader.ReadLong(),
			costs_updates = reader.ReadLong(),
			typ_standard = reader.ReadBool(),
			typ_nachfolger = reader.ReadBool(),
			teile = reader.ReadInt(),
			typ_contractGame = reader.ReadBool(),
			typ_remaster = reader.ReadBool(),
			typ_spinoff = reader.ReadBool(),
			typ_addon = reader.ReadBool(),
			typ_addonStandalone = reader.ReadBool(),
			typ_mmoaddon = reader.ReadBool(),
			typ_bundle = reader.ReadBool(),
			typ_budget = reader.ReadBool(),
			typ_bundleAddon = reader.ReadBool(),
			typ_goty = reader.ReadBool(),
			originalGameID = reader.ReadInt(),
			portID = reader.ReadInt(),
			mainIP = reader.ReadInt(),
			ipPunkte = reader.ReadFloat(),
			exklusiv = reader.ReadBool(),
			herstellerExklusiv = reader.ReadBool(),
			retro = reader.ReadBool(),
			handy = reader.ReadBool(),
			arcade = reader.ReadBool(),
			goty = reader.ReadBool(),
			nachfolger_created = reader.ReadBool(),
			remaster_created = reader.ReadBool(),
			budget_created = reader.ReadBool(),
			goty_created = reader.ReadBool(),
			trendsetter = reader.ReadBool(),
			spielbericht = reader.ReadBool(),
			amountUpdates = reader.ReadInt(),
			bonusSellsUpdates = reader.ReadFloat(),
			amountAddons = reader.ReadInt(),
			bonusSellsAddons = reader.ReadFloat(),
			amountMMOAddons = reader.ReadInt(),
			bonusSellsMMOAddons = reader.ReadFloat(),
			addonQuality = reader.ReadFloat(),
			devAktFeature = reader.ReadInt(),
			devPoints = reader.ReadFloat(),
			devPointsStart = reader.ReadFloat(),
			devPoints_Gesamt = reader.ReadFloat(),
			devPointsStart_Gesamt = reader.ReadFloat(),
			points_gameplay = reader.ReadFloat(),
			points_grafik = reader.ReadFloat(),
			points_sound = reader.ReadFloat(),
			points_technik = reader.ReadFloat(),
			points_bugs = reader.ReadFloat(),
			beschreibung = reader.ReadString(),
			gameTyp = reader.ReadInt(),
			gameSize = reader.ReadInt(),
			gameZielgruppe = reader.ReadInt(),
			maingenre = reader.ReadInt(),
			subgenre = reader.ReadInt(),
			gameMainTheme = reader.ReadInt(),
			gameSubTheme = reader.ReadInt(),
			gameLicence = reader.ReadInt(),
			gameCopyProtect = reader.ReadInt(),
			gameAntiCheat = reader.ReadInt(),
			gameAP_Gameplay = reader.ReadInt(),
			gameAP_Grafik = reader.ReadInt(),
			gameAP_Sound = reader.ReadInt(),
			gameAP_Technik = reader.ReadInt(),
			gameLanguage = _Read_System_002EBoolean_005B_005D(reader),
			gameGameplayFeatures = _Read_System_002EBoolean_005B_005D(reader),
			gamePlatform = _Read_System_002EInt32_005B_005D(reader),
			gameEngineFeature = _Read_System_002EInt32_005B_005D(reader),
			gameplayFeatures_DevDone = _Read_System_002EBoolean_005B_005D(reader),
			engineFeature_DevDone = _Read_System_002EBoolean_005B_005D(reader),
			gameplayStudio = _Read_System_002EBoolean_005B_005D(reader),
			grafikStudio = _Read_System_002EBoolean_005B_005D(reader),
			soundStudio = _Read_System_002EBoolean_005B_005D(reader),
			motionCaptureStudio = _Read_System_002EBoolean_005B_005D(reader),
			bundleID = _Read_System_002EInt32_005B_005D(reader),
			portExist = _Read_System_002EBoolean_005B_005D(reader),
			sellsPerWeek = _Read_System_002EInt32_005B_005D(reader),
			verkaufspreis = _Read_System_002EInt32_005B_005D(reader),
			releaseDate = reader.ReadInt(),
			abonnements = reader.ReadLong(),
			abonnementsWoche = reader.ReadLong(),
			aboPreis = reader.ReadInt(),
			pubOffer = reader.ReadBool(),
			pubAngebot = reader.ReadBool(),
			pubAngebot_Weeks = reader.ReadInt(),
			pubAngebot_Verhandlung = reader.ReadFloat(),
			pubAngebot_Retail = reader.ReadBool(),
			pubAngebot_Digital = reader.ReadBool(),
			pubAngebot_Garantiesumme = reader.ReadInt(),
			pubAngebot_Gewinnbeteiligung = reader.ReadFloat(),
			auftragsspiel = reader.ReadBool(),
			auftragsspiel_gehalt = reader.ReadInt(),
			auftragsspiel_bonus = reader.ReadInt(),
			auftragsspiel_zeitInWochen = reader.ReadInt(),
			auftragsspiel_wochenAlsAngebot = reader.ReadInt(),
			auftragsspiel_zeitAbgelaufen = reader.ReadBool(),
			auftragsspiel_mindestbewertung = reader.ReadInt(),
			f2pConverted = reader.ReadBool(),
			angekuendigt = reader.ReadBool(),
			subvention = reader.ReadLong(),
			sonderIP = reader.ReadBool(),
			sonderIPMindestreview = reader.ReadInt(),
			myNameTeil1 = reader.ReadString(),
			engineGewinnbeteiligung = reader.ReadInt(),
			weeksInDevelopment = reader.ReadInt(),
			userPositiv = reader.ReadInt(),
			userNegativ = reader.ReadInt(),
			bestAbonnements = reader.ReadLong(),
			bestChartPosition = reader.ReadInt(),
			exklusivKonsolenSells = reader.ReadLong(),
			lastChartPosition = reader.ReadInt(),
			freeware = reader.ReadBool(),
			sellsTotalStandard = reader.ReadLong(),
			sellsTotalDeluxe = reader.ReadLong(),
			sellsTotalCollectors = reader.ReadLong(),
			sellsTotalOnline = reader.ReadLong(),
			points_bugsInvis = reader.ReadFloat(),
			umsatzInApp = reader.ReadLong(),
			umsatzAbos = reader.ReadLong(),
			f2pInteresse = reader.ReadFloat(),
			mmoInteresse = reader.ReadFloat(),
			vorbestellungen = reader.ReadInt(),
			realsticPower = reader.ReadFloat(),
			stornierungen = reader.ReadInt(),
			commercialFlop = reader.ReadBool(),
			commercialHit = reader.ReadBool(),
			inAppPurchaseWeek = reader.ReadInt(),
			arcadeCase = reader.ReadInt(),
			arcadeMonitor = reader.ReadInt(),
			arcadeJoystick = reader.ReadInt(),
			arcadeSound = reader.ReadInt(),
			arcadeProdCosts = reader.ReadInt(),
			finanzierung_Grundkosten = reader.ReadInt(),
			finanzierung_Technology = reader.ReadInt(),
			finanzierung_Kontent = reader.ReadInt(),
			retailVersion = reader.ReadBool(),
			digitalVersion = reader.ReadBool(),
			newGenreCombination = reader.ReadBool(),
			newTopicCombination = reader.ReadBool(),
			ipTime = reader.ReadInt(),
			npcLateinNumbers = reader.ReadBool(),
			mmoTOf2p_created = reader.ReadBool(),
			bundle_created = reader.ReadBool(),
			abosAddons = reader.ReadInt(),
			inAppPurchase = _Read_System_002EBoolean_005B_005D(reader),
			Designschwerpunkt = _Read_System_002EInt32_005B_005D(reader),
			Designausrichtung = _Read_System_002EInt32_005B_005D(reader)
		};
	}

	public static void _Write_mpCalls_002Fs_Game(NetworkWriter writer, mpCalls.s_Game value)
	{
		writer.WriteInt(value.gameID);
		writer.WriteString(value.myName);
		writer.WriteString(value.ipName);
		writer.WriteBool(value.playerGame);
		writer.WriteBool(value.inDevelopment);
		writer.WriteInt(value.developerID);
		writer.WriteInt(value.publisherID);
		writer.WriteInt(value.ownerID);
		writer.WriteInt(value.engineID);
		writer.WriteFloat(value.hype);
		writer.WriteBool(value.isOnMarket);
		writer.WriteBool(value.warBeiAwards);
		writer.WriteInt(value.weeksOnMarket);
		writer.WriteInt(value.usk);
		writer.WriteInt(value.freigabeBudget);
		writer.WriteInt(value.reviewGameplay);
		writer.WriteInt(value.reviewGrafik);
		writer.WriteInt(value.reviewSound);
		writer.WriteInt(value.reviewSteuerung);
		writer.WriteInt(value.reviewTotal);
		writer.WriteInt(value.reviewGameplayText);
		writer.WriteInt(value.reviewGrafikText);
		writer.WriteInt(value.reviewSoundText);
		writer.WriteInt(value.reviewSteuerungText);
		writer.WriteInt(value.reviewTotalText);
		writer.WriteInt(value.date_year);
		writer.WriteInt(value.date_month);
		writer.WriteInt(value.date_start_year);
		writer.WriteInt(value.date_start_month);
		writer.WriteLong(value.sellsTotal);
		writer.WriteLong(value.umsatzTotal);
		writer.WriteLong(value.costs_entwicklung);
		writer.WriteLong(value.costs_mitarbeiter);
		writer.WriteLong(value.costs_marketing);
		writer.WriteLong(value.costs_enginegebuehren);
		writer.WriteLong(value.costs_server);
		writer.WriteLong(value.costs_production);
		writer.WriteLong(value.costs_updates);
		writer.WriteBool(value.typ_standard);
		writer.WriteBool(value.typ_nachfolger);
		writer.WriteInt(value.teile);
		writer.WriteBool(value.typ_contractGame);
		writer.WriteBool(value.typ_remaster);
		writer.WriteBool(value.typ_spinoff);
		writer.WriteBool(value.typ_addon);
		writer.WriteBool(value.typ_addonStandalone);
		writer.WriteBool(value.typ_mmoaddon);
		writer.WriteBool(value.typ_bundle);
		writer.WriteBool(value.typ_budget);
		writer.WriteBool(value.typ_bundleAddon);
		writer.WriteBool(value.typ_goty);
		writer.WriteInt(value.originalGameID);
		writer.WriteInt(value.portID);
		writer.WriteInt(value.mainIP);
		writer.WriteFloat(value.ipPunkte);
		writer.WriteBool(value.exklusiv);
		writer.WriteBool(value.herstellerExklusiv);
		writer.WriteBool(value.retro);
		writer.WriteBool(value.handy);
		writer.WriteBool(value.arcade);
		writer.WriteBool(value.goty);
		writer.WriteBool(value.nachfolger_created);
		writer.WriteBool(value.remaster_created);
		writer.WriteBool(value.budget_created);
		writer.WriteBool(value.goty_created);
		writer.WriteBool(value.trendsetter);
		writer.WriteBool(value.spielbericht);
		writer.WriteInt(value.amountUpdates);
		writer.WriteFloat(value.bonusSellsUpdates);
		writer.WriteInt(value.amountAddons);
		writer.WriteFloat(value.bonusSellsAddons);
		writer.WriteInt(value.amountMMOAddons);
		writer.WriteFloat(value.bonusSellsMMOAddons);
		writer.WriteFloat(value.addonQuality);
		writer.WriteInt(value.devAktFeature);
		writer.WriteFloat(value.devPoints);
		writer.WriteFloat(value.devPointsStart);
		writer.WriteFloat(value.devPoints_Gesamt);
		writer.WriteFloat(value.devPointsStart_Gesamt);
		writer.WriteFloat(value.points_gameplay);
		writer.WriteFloat(value.points_grafik);
		writer.WriteFloat(value.points_sound);
		writer.WriteFloat(value.points_technik);
		writer.WriteFloat(value.points_bugs);
		writer.WriteString(value.beschreibung);
		writer.WriteInt(value.gameTyp);
		writer.WriteInt(value.gameSize);
		writer.WriteInt(value.gameZielgruppe);
		writer.WriteInt(value.maingenre);
		writer.WriteInt(value.subgenre);
		writer.WriteInt(value.gameMainTheme);
		writer.WriteInt(value.gameSubTheme);
		writer.WriteInt(value.gameLicence);
		writer.WriteInt(value.gameCopyProtect);
		writer.WriteInt(value.gameAntiCheat);
		writer.WriteInt(value.gameAP_Gameplay);
		writer.WriteInt(value.gameAP_Grafik);
		writer.WriteInt(value.gameAP_Sound);
		writer.WriteInt(value.gameAP_Technik);
		_Write_System_002EBoolean_005B_005D(writer, value.gameLanguage);
		_Write_System_002EBoolean_005B_005D(writer, value.gameGameplayFeatures);
		_Write_System_002EInt32_005B_005D(writer, value.gamePlatform);
		_Write_System_002EInt32_005B_005D(writer, value.gameEngineFeature);
		_Write_System_002EBoolean_005B_005D(writer, value.gameplayFeatures_DevDone);
		_Write_System_002EBoolean_005B_005D(writer, value.engineFeature_DevDone);
		_Write_System_002EBoolean_005B_005D(writer, value.gameplayStudio);
		_Write_System_002EBoolean_005B_005D(writer, value.grafikStudio);
		_Write_System_002EBoolean_005B_005D(writer, value.soundStudio);
		_Write_System_002EBoolean_005B_005D(writer, value.motionCaptureStudio);
		_Write_System_002EInt32_005B_005D(writer, value.bundleID);
		_Write_System_002EBoolean_005B_005D(writer, value.portExist);
		_Write_System_002EInt32_005B_005D(writer, value.sellsPerWeek);
		_Write_System_002EInt32_005B_005D(writer, value.verkaufspreis);
		writer.WriteInt(value.releaseDate);
		writer.WriteLong(value.abonnements);
		writer.WriteLong(value.abonnementsWoche);
		writer.WriteInt(value.aboPreis);
		writer.WriteBool(value.pubOffer);
		writer.WriteBool(value.pubAngebot);
		writer.WriteInt(value.pubAngebot_Weeks);
		writer.WriteFloat(value.pubAngebot_Verhandlung);
		writer.WriteBool(value.pubAngebot_Retail);
		writer.WriteBool(value.pubAngebot_Digital);
		writer.WriteInt(value.pubAngebot_Garantiesumme);
		writer.WriteFloat(value.pubAngebot_Gewinnbeteiligung);
		writer.WriteBool(value.auftragsspiel);
		writer.WriteInt(value.auftragsspiel_gehalt);
		writer.WriteInt(value.auftragsspiel_bonus);
		writer.WriteInt(value.auftragsspiel_zeitInWochen);
		writer.WriteInt(value.auftragsspiel_wochenAlsAngebot);
		writer.WriteBool(value.auftragsspiel_zeitAbgelaufen);
		writer.WriteInt(value.auftragsspiel_mindestbewertung);
		writer.WriteBool(value.f2pConverted);
		writer.WriteBool(value.angekuendigt);
		writer.WriteLong(value.subvention);
		writer.WriteBool(value.sonderIP);
		writer.WriteInt(value.sonderIPMindestreview);
		writer.WriteString(value.myNameTeil1);
		writer.WriteInt(value.engineGewinnbeteiligung);
		writer.WriteInt(value.weeksInDevelopment);
		writer.WriteInt(value.userPositiv);
		writer.WriteInt(value.userNegativ);
		writer.WriteLong(value.bestAbonnements);
		writer.WriteInt(value.bestChartPosition);
		writer.WriteLong(value.exklusivKonsolenSells);
		writer.WriteInt(value.lastChartPosition);
		writer.WriteBool(value.freeware);
		writer.WriteLong(value.sellsTotalStandard);
		writer.WriteLong(value.sellsTotalDeluxe);
		writer.WriteLong(value.sellsTotalCollectors);
		writer.WriteLong(value.sellsTotalOnline);
		writer.WriteFloat(value.points_bugsInvis);
		writer.WriteLong(value.umsatzInApp);
		writer.WriteLong(value.umsatzAbos);
		writer.WriteFloat(value.f2pInteresse);
		writer.WriteFloat(value.mmoInteresse);
		writer.WriteInt(value.vorbestellungen);
		writer.WriteFloat(value.realsticPower);
		writer.WriteInt(value.stornierungen);
		writer.WriteBool(value.commercialFlop);
		writer.WriteBool(value.commercialHit);
		writer.WriteInt(value.inAppPurchaseWeek);
		writer.WriteInt(value.arcadeCase);
		writer.WriteInt(value.arcadeMonitor);
		writer.WriteInt(value.arcadeJoystick);
		writer.WriteInt(value.arcadeSound);
		writer.WriteInt(value.arcadeProdCosts);
		writer.WriteInt(value.finanzierung_Grundkosten);
		writer.WriteInt(value.finanzierung_Technology);
		writer.WriteInt(value.finanzierung_Kontent);
		writer.WriteBool(value.retailVersion);
		writer.WriteBool(value.digitalVersion);
		writer.WriteBool(value.newGenreCombination);
		writer.WriteBool(value.newTopicCombination);
		writer.WriteInt(value.ipTime);
		writer.WriteBool(value.npcLateinNumbers);
		writer.WriteBool(value.mmoTOf2p_created);
		writer.WriteBool(value.bundle_created);
		writer.WriteInt(value.abosAddons);
		_Write_System_002EBoolean_005B_005D(writer, value.inAppPurchase);
		_Write_System_002EInt32_005B_005D(writer, value.Designschwerpunkt);
		_Write_System_002EInt32_005B_005D(writer, value.Designausrichtung);
	}

	public static mpCalls.s_Lizenz _Read_mpCalls_002Fs_Lizenz(NetworkReader reader)
	{
		return new mpCalls.s_Lizenz
		{
			lizenzID = reader.ReadInt(),
			name = reader.ReadString(),
			typ = reader.ReadInt(),
			angebot = reader.ReadInt(),
			quality = reader.ReadFloat(),
			licence_GENREGOOD = reader.ReadInt(),
			licence_GENREBAD = reader.ReadInt(),
			licence_YEAR = reader.ReadInt()
		};
	}

	public static void _Write_mpCalls_002Fs_Lizenz(NetworkWriter writer, mpCalls.s_Lizenz value)
	{
		writer.WriteInt(value.lizenzID);
		writer.WriteString(value.name);
		writer.WriteInt(value.typ);
		writer.WriteInt(value.angebot);
		writer.WriteFloat(value.quality);
		writer.WriteInt(value.licence_GENREGOOD);
		writer.WriteInt(value.licence_GENREBAD);
		writer.WriteInt(value.licence_YEAR);
	}

	public static mpCalls.s_Trend _Read_mpCalls_002Fs_Trend(NetworkReader reader)
	{
		return new mpCalls.s_Trend
		{
			trendWeeks = reader.ReadInt(),
			trendTheme = reader.ReadInt(),
			trendAntiTheme = reader.ReadInt(),
			trendGenre = reader.ReadInt(),
			trendAntiGenre = reader.ReadInt(),
			trendNextGenre = reader.ReadInt(),
			trendNextAntiGenre = reader.ReadInt(),
			trendNextTheme = reader.ReadInt(),
			trendNextAntiTheme = reader.ReadInt()
		};
	}

	public static void _Write_mpCalls_002Fs_Trend(NetworkWriter writer, mpCalls.s_Trend value)
	{
		writer.WriteInt(value.trendWeeks);
		writer.WriteInt(value.trendTheme);
		writer.WriteInt(value.trendAntiTheme);
		writer.WriteInt(value.trendGenre);
		writer.WriteInt(value.trendAntiGenre);
		writer.WriteInt(value.trendNextGenre);
		writer.WriteInt(value.trendNextAntiGenre);
		writer.WriteInt(value.trendNextTheme);
		writer.WriteInt(value.trendNextAntiTheme);
	}

	public static mpCalls.s_GameSpeed _Read_mpCalls_002Fs_GameSpeed(NetworkReader reader)
	{
		return new mpCalls.s_GameSpeed
		{
			speed = reader.ReadInt()
		};
	}

	public static void _Write_mpCalls_002Fs_GameSpeed(NetworkWriter writer, mpCalls.s_GameSpeed value)
	{
		writer.WriteInt(value.speed);
	}

	public static mpCalls.s_Command _Read_mpCalls_002Fs_Command(NetworkReader reader)
	{
		return new mpCalls.s_Command
		{
			command = reader.ReadInt()
		};
	}

	public static void _Write_mpCalls_002Fs_Command(NetworkWriter writer, mpCalls.s_Command value)
	{
		writer.WriteInt(value.command);
	}

	public static mpCalls.s_Save _Read_mpCalls_002Fs_Save(NetworkReader reader)
	{
		return new mpCalls.s_Save
		{
			saveID = reader.ReadInt()
		};
	}

	public static void _Write_mpCalls_002Fs_Save(NetworkWriter writer, mpCalls.s_Save value)
	{
		writer.WriteInt(value.saveID);
	}

	public static mpCalls.s_Load _Read_mpCalls_002Fs_Load(NetworkReader reader)
	{
		return new mpCalls.s_Load
		{
			saveID = reader.ReadInt()
		};
	}

	public static void _Write_mpCalls_002Fs_Load(NetworkWriter writer, mpCalls.s_Load value)
	{
		writer.WriteInt(value.saveID);
	}

	public static mpCalls.s_PlayerID _Read_mpCalls_002Fs_PlayerID(NetworkReader reader)
	{
		return new mpCalls.s_PlayerID
		{
			id = reader.ReadInt(),
			version = reader.ReadString()
		};
	}

	public static void _Write_mpCalls_002Fs_PlayerID(NetworkWriter writer, mpCalls.s_PlayerID value)
	{
		writer.WriteInt(value.id);
		writer.WriteString(value.version);
	}

	public static mpCalls.s_PlayerInfos _Read_mpCalls_002Fs_PlayerInfos(NetworkReader reader)
	{
		return new mpCalls.s_PlayerInfos
		{
			id = reader.ReadInt(),
			playerName = reader.ReadString(),
			ready = reader.ReadBool()
		};
	}

	public static void _Write_mpCalls_002Fs_PlayerInfos(NetworkWriter writer, mpCalls.s_PlayerInfos value)
	{
		writer.WriteInt(value.id);
		writer.WriteString(value.playerName);
		writer.WriteBool(value.ready);
	}

	public static mpCalls.s_KillAA _Read_mpCalls_002Fs_KillAA(NetworkReader reader)
	{
		return new mpCalls.s_KillAA
		{
			charID = reader.ReadInt(),
			wert2 = reader.ReadInt(),
			eingestellt = reader.ReadBool(),
			wert3 = reader.ReadInt()
		};
	}

	public static void _Write_mpCalls_002Fs_KillAA(NetworkWriter writer, mpCalls.s_KillAA value)
	{
		writer.WriteInt(value.charID);
		writer.WriteInt(value.wert2);
		writer.WriteBool(value.eingestellt);
		writer.WriteInt(value.wert3);
	}

	public static mpCalls.s_CreateArbeitsmarkt _Read_mpCalls_002Fs_CreateArbeitsmarkt(NetworkReader reader)
	{
		return new mpCalls.s_CreateArbeitsmarkt
		{
			objectID = reader.ReadInt(),
			male = reader.ReadBool(),
			myName = reader.ReadString(),
			wochenAmArbeitsmarkt = reader.ReadInt(),
			legend = reader.ReadInt(),
			beruf = reader.ReadInt(),
			s_gamedesign = reader.ReadFloat(),
			s_programmieren = reader.ReadFloat(),
			s_grafik = reader.ReadFloat(),
			s_sound = reader.ReadFloat(),
			s_pr = reader.ReadFloat(),
			s_gametests = reader.ReadFloat(),
			s_technik = reader.ReadFloat(),
			s_forschen = reader.ReadFloat(),
			perks = _Read_System_002EBoolean_005B_005D(reader),
			model_body = reader.ReadInt(),
			model_eyes = reader.ReadInt(),
			model_hair = reader.ReadInt(),
			model_beard = reader.ReadInt(),
			model_skinColor = reader.ReadInt(),
			model_hairColor = reader.ReadInt(),
			model_beardColor = reader.ReadInt(),
			model_HoseColor = reader.ReadInt(),
			model_ShirtColor = reader.ReadInt(),
			model_Add1Color = reader.ReadInt()
		};
	}

	public static void _Write_mpCalls_002Fs_CreateArbeitsmarkt(NetworkWriter writer, mpCalls.s_CreateArbeitsmarkt value)
	{
		writer.WriteInt(value.objectID);
		writer.WriteBool(value.male);
		writer.WriteString(value.myName);
		writer.WriteInt(value.wochenAmArbeitsmarkt);
		writer.WriteInt(value.legend);
		writer.WriteInt(value.beruf);
		writer.WriteFloat(value.s_gamedesign);
		writer.WriteFloat(value.s_programmieren);
		writer.WriteFloat(value.s_grafik);
		writer.WriteFloat(value.s_sound);
		writer.WriteFloat(value.s_pr);
		writer.WriteFloat(value.s_gametests);
		writer.WriteFloat(value.s_technik);
		writer.WriteFloat(value.s_forschen);
		_Write_System_002EBoolean_005B_005D(writer, value.perks);
		writer.WriteInt(value.model_body);
		writer.WriteInt(value.model_eyes);
		writer.WriteInt(value.model_hair);
		writer.WriteInt(value.model_beard);
		writer.WriteInt(value.model_skinColor);
		writer.WriteInt(value.model_hairColor);
		writer.WriteInt(value.model_beardColor);
		writer.WriteInt(value.model_HoseColor);
		writer.WriteInt(value.model_ShirtColor);
		writer.WriteInt(value.model_Add1Color);
	}

	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
	public static void InitReadWriters()
	{
		Writer<byte>.write = NetworkWriterExtensions.WriteByte;
		Writer<byte?>.write = NetworkWriterExtensions.WriteByteNullable;
		Writer<sbyte>.write = NetworkWriterExtensions.WriteSByte;
		Writer<sbyte?>.write = NetworkWriterExtensions.WriteSByteNullable;
		Writer<char>.write = NetworkWriterExtensions.WriteChar;
		Writer<char?>.write = NetworkWriterExtensions.WriteCharNullable;
		Writer<bool>.write = NetworkWriterExtensions.WriteBool;
		Writer<bool?>.write = NetworkWriterExtensions.WriteBoolNullable;
		Writer<short>.write = NetworkWriterExtensions.WriteShort;
		Writer<short?>.write = NetworkWriterExtensions.WriteShortNullable;
		Writer<ushort>.write = NetworkWriterExtensions.WriteUShort;
		Writer<ushort?>.write = NetworkWriterExtensions.WriteUShortNullable;
		Writer<int>.write = NetworkWriterExtensions.WriteInt;
		Writer<int?>.write = NetworkWriterExtensions.WriteIntNullable;
		Writer<uint>.write = NetworkWriterExtensions.WriteUInt;
		Writer<uint?>.write = NetworkWriterExtensions.WriteUIntNullable;
		Writer<long>.write = NetworkWriterExtensions.WriteLong;
		Writer<long?>.write = NetworkWriterExtensions.WriteLongNullable;
		Writer<ulong>.write = NetworkWriterExtensions.WriteULong;
		Writer<ulong?>.write = NetworkWriterExtensions.WriteULongNullable;
		Writer<float>.write = NetworkWriterExtensions.WriteFloat;
		Writer<float?>.write = NetworkWriterExtensions.WriteFloatNullable;
		Writer<double>.write = NetworkWriterExtensions.WriteDouble;
		Writer<double?>.write = NetworkWriterExtensions.WriteDoubleNullable;
		Writer<decimal>.write = NetworkWriterExtensions.WriteDecimal;
		Writer<decimal?>.write = NetworkWriterExtensions.WriteDecimalNullable;
		Writer<string>.write = NetworkWriterExtensions.WriteString;
		Writer<ArraySegment<byte>>.write = NetworkWriterExtensions.WriteBytesAndSizeSegment;
		Writer<byte[]>.write = NetworkWriterExtensions.WriteBytesAndSize;
		Writer<Vector2>.write = NetworkWriterExtensions.WriteVector2;
		Writer<Vector2?>.write = NetworkWriterExtensions.WriteVector2Nullable;
		Writer<Vector3>.write = NetworkWriterExtensions.WriteVector3;
		Writer<Vector3?>.write = NetworkWriterExtensions.WriteVector3Nullable;
		Writer<Vector4>.write = NetworkWriterExtensions.WriteVector4;
		Writer<Vector4?>.write = NetworkWriterExtensions.WriteVector4Nullable;
		Writer<Vector2Int>.write = NetworkWriterExtensions.WriteVector2Int;
		Writer<Vector2Int?>.write = NetworkWriterExtensions.WriteVector2IntNullable;
		Writer<Vector3Int>.write = NetworkWriterExtensions.WriteVector3Int;
		Writer<Vector3Int?>.write = NetworkWriterExtensions.WriteVector3IntNullable;
		Writer<Color>.write = NetworkWriterExtensions.WriteColor;
		Writer<Color?>.write = NetworkWriterExtensions.WriteColorNullable;
		Writer<Color32>.write = NetworkWriterExtensions.WriteColor32;
		Writer<Color32?>.write = NetworkWriterExtensions.WriteColor32Nullable;
		Writer<Quaternion>.write = NetworkWriterExtensions.WriteQuaternion;
		Writer<Quaternion?>.write = NetworkWriterExtensions.WriteQuaternionNullable;
		Writer<Rect>.write = NetworkWriterExtensions.WriteRect;
		Writer<Rect?>.write = NetworkWriterExtensions.WriteRectNullable;
		Writer<Plane>.write = NetworkWriterExtensions.WritePlane;
		Writer<Plane?>.write = NetworkWriterExtensions.WritePlaneNullable;
		Writer<Ray>.write = NetworkWriterExtensions.WriteRay;
		Writer<Ray?>.write = NetworkWriterExtensions.WriteRayNullable;
		Writer<Matrix4x4>.write = NetworkWriterExtensions.WriteMatrix4x4;
		Writer<Matrix4x4?>.write = NetworkWriterExtensions.WriteMatrix4x4Nullable;
		Writer<Guid>.write = NetworkWriterExtensions.WriteGuid;
		Writer<Guid?>.write = NetworkWriterExtensions.WriteGuidNullable;
		Writer<NetworkIdentity>.write = NetworkWriterExtensions.WriteNetworkIdentity;
		Writer<NetworkBehaviour>.write = NetworkWriterExtensions.WriteNetworkBehaviour;
		Writer<Transform>.write = NetworkWriterExtensions.WriteTransform;
		Writer<GameObject>.write = NetworkWriterExtensions.WriteGameObject;
		Writer<Uri>.write = NetworkWriterExtensions.WriteUri;
		Writer<Texture2D>.write = NetworkWriterExtensions.WriteTexture2D;
		Writer<Sprite>.write = NetworkWriterExtensions.WriteSprite;
		Writer<DateTime>.write = NetworkWriterExtensions.WriteDateTime;
		Writer<DateTime?>.write = NetworkWriterExtensions.WriteDateTimeNullable;
		Writer<TimeSnapshotMessage>.write = _Write_Mirror_002ETimeSnapshotMessage;
		Writer<ReadyMessage>.write = _Write_Mirror_002EReadyMessage;
		Writer<NotReadyMessage>.write = _Write_Mirror_002ENotReadyMessage;
		Writer<AddPlayerMessage>.write = _Write_Mirror_002EAddPlayerMessage;
		Writer<SceneMessage>.write = _Write_Mirror_002ESceneMessage;
		Writer<SceneOperation>.write = _Write_Mirror_002ESceneOperation;
		Writer<CommandMessage>.write = _Write_Mirror_002ECommandMessage;
		Writer<RpcMessage>.write = _Write_Mirror_002ERpcMessage;
		Writer<RpcBufferMessage>.write = _Write_Mirror_002ERpcBufferMessage;
		Writer<SpawnMessage>.write = _Write_Mirror_002ESpawnMessage;
		Writer<ChangeOwnerMessage>.write = _Write_Mirror_002EChangeOwnerMessage;
		Writer<ObjectSpawnStartedMessage>.write = _Write_Mirror_002EObjectSpawnStartedMessage;
		Writer<ObjectSpawnFinishedMessage>.write = _Write_Mirror_002EObjectSpawnFinishedMessage;
		Writer<ObjectDestroyMessage>.write = _Write_Mirror_002EObjectDestroyMessage;
		Writer<ObjectHideMessage>.write = _Write_Mirror_002EObjectHideMessage;
		Writer<EntityStateMessage>.write = _Write_Mirror_002EEntityStateMessage;
		Writer<NetworkPingMessage>.write = _Write_Mirror_002ENetworkPingMessage;
		Writer<NetworkPongMessage>.write = _Write_Mirror_002ENetworkPongMessage;
		Writer<mpCalls.c_GameSpeed>.write = _Write_mpCalls_002Fc_GameSpeed;
		Writer<mpCalls.c_NpcGameName>.write = _Write_mpCalls_002Fc_NpcGameName;
		Writer<mpCalls.c_BlockContractGame>.write = _Write_mpCalls_002Fc_BlockContractGame;
		Writer<mpCalls.c_Publisher>.write = _Write_mpCalls_002Fc_Publisher;
		Writer<int[]>.write = _Write_System_002EInt32_005B_005D;
		Writer<mpCalls.c_PublisherOwner>.write = _Write_mpCalls_002Fc_PublisherOwner;
		Writer<mpCalls.c_PublisherTochterfirmaSettings>.write = _Write_mpCalls_002Fc_PublisherTochterfirmaSettings;
		Writer<mpCalls.c_Forschung>.write = _Write_mpCalls_002Fc_Forschung;
		Writer<bool[]>.write = _Write_System_002EBoolean_005B_005D;
		Writer<mpCalls.c_Help>.write = _Write_mpCalls_002Fc_Help;
		Writer<mpCalls.c_ObjectDelete>.write = _Write_mpCalls_002Fc_ObjectDelete;
		Writer<mpCalls.c_Object>.write = _Write_mpCalls_002Fc_Object;
		Writer<mpCalls.c_Map>.write = _Write_mpCalls_002Fc_Map;
		Writer<mpCalls.c_Trend>.write = _Write_mpCalls_002Fc_Trend;
		Writer<mpCalls.c_Payment>.write = _Write_mpCalls_002Fc_Payment;
		Writer<mpCalls.c_Engine>.write = _Write_mpCalls_002Fc_Engine;
		Writer<mpCalls.c_EnginePublisherBuyed>.write = _Write_mpCalls_002Fc_EnginePublisherBuyed;
		Writer<mpCalls.c_Platform>.write = _Write_mpCalls_002Fc_Platform;
		Writer<mpCalls.c_PlatformRemoveFromMarket>.write = _Write_mpCalls_002Fc_PlatformRemoveFromMarket;
		Writer<mpCalls.c_Chat>.write = _Write_mpCalls_002Fc_Chat;
		Writer<mpCalls.c_Command>.write = _Write_mpCalls_002Fc_Command;
		Writer<mpCalls.c_Money>.write = _Write_mpCalls_002Fc_Money;
		Writer<mpCalls.c_PlayerInfos>.write = _Write_mpCalls_002Fc_PlayerInfos;
		Writer<mpCalls.c_DeleteArbeitsmarkt>.write = _Write_mpCalls_002Fc_DeleteArbeitsmarkt;
		Writer<mpCalls.c_BuyLizenz>.write = _Write_mpCalls_002Fc_BuyLizenz;
		Writer<mpCalls.c_exklusivKonsolenSells>.write = _Write_mpCalls_002Fc_exklusivKonsolenSells;
		Writer<mpCalls.c_GameDestroy>.write = _Write_mpCalls_002Fc_GameDestroy;
		Writer<mpCalls.c_GameRemoveFromMarket>.write = _Write_mpCalls_002Fc_GameRemoveFromMarket;
		Writer<mpCalls.c_GameOwner>.write = _Write_mpCalls_002Fc_GameOwner;
		Writer<mpCalls.c_GameIpSell>.write = _Write_mpCalls_002Fc_GameIpSell;
		Writer<mpCalls.c_GameIpPoints>.write = _Write_mpCalls_002Fc_GameIpPoints;
		Writer<mpCalls.c_GameSell>.write = _Write_mpCalls_002Fc_GameSell;
		Writer<mpCalls.c_Game>.write = _Write_mpCalls_002Fc_Game;
		Writer<mpCalls.s_AddPlayer>.write = _Write_mpCalls_002Fs_AddPlayer;
		Writer<mpCalls.s_Forschung>.write = _Write_mpCalls_002Fs_Forschung;
		Writer<mpCalls.s_PlayerLeave>.write = _Write_mpCalls_002Fs_PlayerLeave;
		Writer<mpCalls.s_GenreBeliebtheit>.write = _Write_mpCalls_002Fs_GenreBeliebtheit;
		Writer<float[]>.write = _Write_System_002ESingle_005B_005D;
		Writer<mpCalls.s_GenreCombination>.write = _Write_mpCalls_002Fs_GenreCombination;
		Writer<mpCalls.s_GenreDesign>.write = _Write_mpCalls_002Fs_GenreDesign;
		Writer<mpCalls.s_GenrePlatformSuit>.write = _Write_mpCalls_002Fs_GenrePlatformSuit;
		Writer<mpCalls.s_Help>.write = _Write_mpCalls_002Fs_Help;
		Writer<mpCalls.s_ObjectDelete>.write = _Write_mpCalls_002Fs_ObjectDelete;
		Writer<mpCalls.s_Object>.write = _Write_mpCalls_002Fs_Object;
		Writer<mpCalls.s_Map>.write = _Write_mpCalls_002Fs_Map;
		Writer<mpCalls.s_Office>.write = _Write_mpCalls_002Fs_Office;
		Writer<mpCalls.s_Difficulty>.write = _Write_mpCalls_002Fs_Difficulty;
		Writer<mpCalls.s_RandomSettings>.write = _Write_mpCalls_002Fs_RandomSettings;
		Writer<mpCalls.s_Wettbewerb>.write = _Write_mpCalls_002Fs_Wettbewerb;
		Writer<mpCalls.s_Startjahr>.write = _Write_mpCalls_002Fs_Startjahr;
		Writer<mpCalls.s_Entwicklungsdauer>.write = _Write_mpCalls_002Fs_Entwicklungsdauer;
		Writer<mpCalls.s_AnzahlKonkurrenten>.write = _Write_mpCalls_002Fs_AnzahlKonkurrenten;
		Writer<mpCalls.s_Spielgeschwindigkeit>.write = _Write_mpCalls_002Fs_Spielgeschwindigkeit;
		Writer<mpCalls.s_GlobalEvent>.write = _Write_mpCalls_002Fs_GlobalEvent;
		Writer<mpCalls.s_EngineAbrechnung>.write = _Write_mpCalls_002Fs_EngineAbrechnung;
		Writer<mpCalls.s_Awards>.write = _Write_mpCalls_002Fs_Awards;
		Writer<mpCalls.s_Payment>.write = _Write_mpCalls_002Fs_Payment;
		Writer<mpCalls.s_Engine>.write = _Write_mpCalls_002Fs_Engine;
		Writer<mpCalls.s_EnginePublisherBuyed>.write = _Write_mpCalls_002Fs_EnginePublisherBuyed;
		Writer<mpCalls.s_Platform>.write = _Write_mpCalls_002Fs_Platform;
		Writer<mpCalls.s_PlatformData>.write = _Write_mpCalls_002Fs_PlatformData;
		Writer<mpCalls.s_PlatformRemoveFromMarket>.write = _Write_mpCalls_002Fs_PlatformRemoveFromMarket;
		Writer<mpCalls.s_PlatformSubvention>.write = _Write_mpCalls_002Fs_PlatformSubvention;
		Writer<mpCalls.s_Chat>.write = _Write_mpCalls_002Fs_Chat;
		Writer<mpCalls.s_Money>.write = _Write_mpCalls_002Fs_Money;
		Writer<mpCalls.s_AutoPause>.write = _Write_mpCalls_002Fs_AutoPause;
		Writer<mpCalls.s_Genres>.write = _Write_mpCalls_002Fs_Genres;
		Writer<string[]>.write = _Write_System_002EString_005B_005D;
		Writer<mpCalls.s_Topics>.write = _Write_mpCalls_002Fs_Topics;
		Writer<mpCalls.s_GameplayFeatures>.write = _Write_mpCalls_002Fs_GameplayFeatures;
		Writer<mpCalls.s_EngineFeatures>.write = _Write_mpCalls_002Fs_EngineFeatures;
		Writer<mpCalls.s_HardwareFeatures>.write = _Write_mpCalls_002Fs_HardwareFeatures;
		Writer<mpCalls.s_Hardware>.write = _Write_mpCalls_002Fs_Hardware;
		Writer<mpCalls.s_AntiCheat>.write = _Write_mpCalls_002Fs_AntiCheat;
		Writer<mpCalls.s_CopyProtect>.write = _Write_mpCalls_002Fs_CopyProtect;
		Writer<mpCalls.s_NpcEngine>.write = _Write_mpCalls_002Fs_NpcEngine;
		Writer<mpCalls.s_TochterfirmaUmsatz>.write = _Write_mpCalls_002Fs_TochterfirmaUmsatz;
		Writer<mpCalls.s_Firmenwert>.write = _Write_mpCalls_002Fs_Firmenwert;
		Writer<long[]>.write = _Write_System_002EInt64_005B_005D;
		Writer<mpCalls.s_NpcGameName>.write = _Write_mpCalls_002Fs_NpcGameName;
		Writer<mpCalls.s_BlockContractGame>.write = _Write_mpCalls_002Fs_BlockContractGame;
		Writer<mpCalls.s_Publisher>.write = _Write_mpCalls_002Fs_Publisher;
		Writer<mpCalls.s_PublisherOwner>.write = _Write_mpCalls_002Fs_PublisherOwner;
		Writer<mpCalls.s_PublisherClose>.write = _Write_mpCalls_002Fs_PublisherClose;
		Writer<mpCalls.s_PublisherTochterfirmaSettings>.write = _Write_mpCalls_002Fs_PublisherTochterfirmaSettings;
		Writer<mpCalls.s_exklusivKonsolenSells>.write = _Write_mpCalls_002Fs_exklusivKonsolenSells;
		Writer<mpCalls.s_GameAnkuendigung>.write = _Write_mpCalls_002Fs_GameAnkuendigung;
		Writer<mpCalls.s_GameDestroy>.write = _Write_mpCalls_002Fs_GameDestroy;
		Writer<mpCalls.s_GameRemoveFromMarket>.write = _Write_mpCalls_002Fs_GameRemoveFromMarket;
		Writer<mpCalls.s_GameOwner>.write = _Write_mpCalls_002Fs_GameOwner;
		Writer<mpCalls.s_GameIpSell>.write = _Write_mpCalls_002Fs_GameIpSell;
		Writer<mpCalls.s_GameIpPoints>.write = _Write_mpCalls_002Fs_GameIpPoints;
		Writer<mpCalls.s_GameSell>.write = _Write_mpCalls_002Fs_GameSell;
		Writer<mpCalls.s_Game>.write = _Write_mpCalls_002Fs_Game;
		Writer<mpCalls.s_Lizenz>.write = _Write_mpCalls_002Fs_Lizenz;
		Writer<mpCalls.s_Trend>.write = _Write_mpCalls_002Fs_Trend;
		Writer<mpCalls.s_GameSpeed>.write = _Write_mpCalls_002Fs_GameSpeed;
		Writer<mpCalls.s_Command>.write = _Write_mpCalls_002Fs_Command;
		Writer<mpCalls.s_Save>.write = _Write_mpCalls_002Fs_Save;
		Writer<mpCalls.s_Load>.write = _Write_mpCalls_002Fs_Load;
		Writer<mpCalls.s_PlayerID>.write = _Write_mpCalls_002Fs_PlayerID;
		Writer<mpCalls.s_PlayerInfos>.write = _Write_mpCalls_002Fs_PlayerInfos;
		Writer<mpCalls.s_KillAA>.write = _Write_mpCalls_002Fs_KillAA;
		Writer<mpCalls.s_CreateArbeitsmarkt>.write = _Write_mpCalls_002Fs_CreateArbeitsmarkt;
		Reader<byte>.read = NetworkReaderExtensions.ReadByte;
		Reader<byte?>.read = NetworkReaderExtensions.ReadByteNullable;
		Reader<sbyte>.read = NetworkReaderExtensions.ReadSByte;
		Reader<sbyte?>.read = NetworkReaderExtensions.ReadSByteNullable;
		Reader<char>.read = NetworkReaderExtensions.ReadChar;
		Reader<char?>.read = NetworkReaderExtensions.ReadCharNullable;
		Reader<bool>.read = NetworkReaderExtensions.ReadBool;
		Reader<bool?>.read = NetworkReaderExtensions.ReadBoolNullable;
		Reader<short>.read = NetworkReaderExtensions.ReadShort;
		Reader<short?>.read = NetworkReaderExtensions.ReadShortNullable;
		Reader<ushort>.read = NetworkReaderExtensions.ReadUShort;
		Reader<ushort?>.read = NetworkReaderExtensions.ReadUShortNullable;
		Reader<int>.read = NetworkReaderExtensions.ReadInt;
		Reader<int?>.read = NetworkReaderExtensions.ReadIntNullable;
		Reader<uint>.read = NetworkReaderExtensions.ReadUInt;
		Reader<uint?>.read = NetworkReaderExtensions.ReadUIntNullable;
		Reader<long>.read = NetworkReaderExtensions.ReadLong;
		Reader<long?>.read = NetworkReaderExtensions.ReadLongNullable;
		Reader<ulong>.read = NetworkReaderExtensions.ReadULong;
		Reader<ulong?>.read = NetworkReaderExtensions.ReadULongNullable;
		Reader<float>.read = NetworkReaderExtensions.ReadFloat;
		Reader<float?>.read = NetworkReaderExtensions.ReadFloatNullable;
		Reader<double>.read = NetworkReaderExtensions.ReadDouble;
		Reader<double?>.read = NetworkReaderExtensions.ReadDoubleNullable;
		Reader<decimal>.read = NetworkReaderExtensions.ReadDecimal;
		Reader<decimal?>.read = NetworkReaderExtensions.ReadDecimalNullable;
		Reader<string>.read = NetworkReaderExtensions.ReadString;
		Reader<byte[]>.read = NetworkReaderExtensions.ReadBytesAndSize;
		Reader<ArraySegment<byte>>.read = NetworkReaderExtensions.ReadBytesAndSizeSegment;
		Reader<Vector2>.read = NetworkReaderExtensions.ReadVector2;
		Reader<Vector2?>.read = NetworkReaderExtensions.ReadVector2Nullable;
		Reader<Vector3>.read = NetworkReaderExtensions.ReadVector3;
		Reader<Vector3?>.read = NetworkReaderExtensions.ReadVector3Nullable;
		Reader<Vector4>.read = NetworkReaderExtensions.ReadVector4;
		Reader<Vector4?>.read = NetworkReaderExtensions.ReadVector4Nullable;
		Reader<Vector2Int>.read = NetworkReaderExtensions.ReadVector2Int;
		Reader<Vector2Int?>.read = NetworkReaderExtensions.ReadVector2IntNullable;
		Reader<Vector3Int>.read = NetworkReaderExtensions.ReadVector3Int;
		Reader<Vector3Int?>.read = NetworkReaderExtensions.ReadVector3IntNullable;
		Reader<Color>.read = NetworkReaderExtensions.ReadColor;
		Reader<Color?>.read = NetworkReaderExtensions.ReadColorNullable;
		Reader<Color32>.read = NetworkReaderExtensions.ReadColor32;
		Reader<Color32?>.read = NetworkReaderExtensions.ReadColor32Nullable;
		Reader<Quaternion>.read = NetworkReaderExtensions.ReadQuaternion;
		Reader<Quaternion?>.read = NetworkReaderExtensions.ReadQuaternionNullable;
		Reader<Rect>.read = NetworkReaderExtensions.ReadRect;
		Reader<Rect?>.read = NetworkReaderExtensions.ReadRectNullable;
		Reader<Plane>.read = NetworkReaderExtensions.ReadPlane;
		Reader<Plane?>.read = NetworkReaderExtensions.ReadPlaneNullable;
		Reader<Ray>.read = NetworkReaderExtensions.ReadRay;
		Reader<Ray?>.read = NetworkReaderExtensions.ReadRayNullable;
		Reader<Matrix4x4>.read = NetworkReaderExtensions.ReadMatrix4x4;
		Reader<Matrix4x4?>.read = NetworkReaderExtensions.ReadMatrix4x4Nullable;
		Reader<Guid>.read = NetworkReaderExtensions.ReadGuid;
		Reader<Guid?>.read = NetworkReaderExtensions.ReadGuidNullable;
		Reader<NetworkIdentity>.read = NetworkReaderExtensions.ReadNetworkIdentity;
		Reader<NetworkBehaviour>.read = NetworkReaderExtensions.ReadNetworkBehaviour;
		Reader<NetworkBehaviourSyncVar>.read = NetworkReaderExtensions.ReadNetworkBehaviourSyncVar;
		Reader<Transform>.read = NetworkReaderExtensions.ReadTransform;
		Reader<GameObject>.read = NetworkReaderExtensions.ReadGameObject;
		Reader<Uri>.read = NetworkReaderExtensions.ReadUri;
		Reader<Texture2D>.read = NetworkReaderExtensions.ReadTexture2D;
		Reader<Sprite>.read = NetworkReaderExtensions.ReadSprite;
		Reader<DateTime>.read = NetworkReaderExtensions.ReadDateTime;
		Reader<DateTime?>.read = NetworkReaderExtensions.ReadDateTimeNullable;
		Reader<TimeSnapshotMessage>.read = _Read_Mirror_002ETimeSnapshotMessage;
		Reader<ReadyMessage>.read = _Read_Mirror_002EReadyMessage;
		Reader<NotReadyMessage>.read = _Read_Mirror_002ENotReadyMessage;
		Reader<AddPlayerMessage>.read = _Read_Mirror_002EAddPlayerMessage;
		Reader<SceneMessage>.read = _Read_Mirror_002ESceneMessage;
		Reader<SceneOperation>.read = _Read_Mirror_002ESceneOperation;
		Reader<CommandMessage>.read = _Read_Mirror_002ECommandMessage;
		Reader<RpcMessage>.read = _Read_Mirror_002ERpcMessage;
		Reader<RpcBufferMessage>.read = _Read_Mirror_002ERpcBufferMessage;
		Reader<SpawnMessage>.read = _Read_Mirror_002ESpawnMessage;
		Reader<ChangeOwnerMessage>.read = _Read_Mirror_002EChangeOwnerMessage;
		Reader<ObjectSpawnStartedMessage>.read = _Read_Mirror_002EObjectSpawnStartedMessage;
		Reader<ObjectSpawnFinishedMessage>.read = _Read_Mirror_002EObjectSpawnFinishedMessage;
		Reader<ObjectDestroyMessage>.read = _Read_Mirror_002EObjectDestroyMessage;
		Reader<ObjectHideMessage>.read = _Read_Mirror_002EObjectHideMessage;
		Reader<EntityStateMessage>.read = _Read_Mirror_002EEntityStateMessage;
		Reader<NetworkPingMessage>.read = _Read_Mirror_002ENetworkPingMessage;
		Reader<NetworkPongMessage>.read = _Read_Mirror_002ENetworkPongMessage;
		Reader<mpCalls.c_GameSpeed>.read = _Read_mpCalls_002Fc_GameSpeed;
		Reader<mpCalls.c_NpcGameName>.read = _Read_mpCalls_002Fc_NpcGameName;
		Reader<mpCalls.c_BlockContractGame>.read = _Read_mpCalls_002Fc_BlockContractGame;
		Reader<mpCalls.c_Publisher>.read = _Read_mpCalls_002Fc_Publisher;
		Reader<int[]>.read = _Read_System_002EInt32_005B_005D;
		Reader<mpCalls.c_PublisherOwner>.read = _Read_mpCalls_002Fc_PublisherOwner;
		Reader<mpCalls.c_PublisherTochterfirmaSettings>.read = _Read_mpCalls_002Fc_PublisherTochterfirmaSettings;
		Reader<mpCalls.c_Forschung>.read = _Read_mpCalls_002Fc_Forschung;
		Reader<bool[]>.read = _Read_System_002EBoolean_005B_005D;
		Reader<mpCalls.c_Help>.read = _Read_mpCalls_002Fc_Help;
		Reader<mpCalls.c_ObjectDelete>.read = _Read_mpCalls_002Fc_ObjectDelete;
		Reader<mpCalls.c_Object>.read = _Read_mpCalls_002Fc_Object;
		Reader<mpCalls.c_Map>.read = _Read_mpCalls_002Fc_Map;
		Reader<mpCalls.c_Trend>.read = _Read_mpCalls_002Fc_Trend;
		Reader<mpCalls.c_Payment>.read = _Read_mpCalls_002Fc_Payment;
		Reader<mpCalls.c_Engine>.read = _Read_mpCalls_002Fc_Engine;
		Reader<mpCalls.c_EnginePublisherBuyed>.read = _Read_mpCalls_002Fc_EnginePublisherBuyed;
		Reader<mpCalls.c_Platform>.read = _Read_mpCalls_002Fc_Platform;
		Reader<mpCalls.c_PlatformRemoveFromMarket>.read = _Read_mpCalls_002Fc_PlatformRemoveFromMarket;
		Reader<mpCalls.c_Chat>.read = _Read_mpCalls_002Fc_Chat;
		Reader<mpCalls.c_Command>.read = _Read_mpCalls_002Fc_Command;
		Reader<mpCalls.c_Money>.read = _Read_mpCalls_002Fc_Money;
		Reader<mpCalls.c_PlayerInfos>.read = _Read_mpCalls_002Fc_PlayerInfos;
		Reader<mpCalls.c_DeleteArbeitsmarkt>.read = _Read_mpCalls_002Fc_DeleteArbeitsmarkt;
		Reader<mpCalls.c_BuyLizenz>.read = _Read_mpCalls_002Fc_BuyLizenz;
		Reader<mpCalls.c_exklusivKonsolenSells>.read = _Read_mpCalls_002Fc_exklusivKonsolenSells;
		Reader<mpCalls.c_GameDestroy>.read = _Read_mpCalls_002Fc_GameDestroy;
		Reader<mpCalls.c_GameRemoveFromMarket>.read = _Read_mpCalls_002Fc_GameRemoveFromMarket;
		Reader<mpCalls.c_GameOwner>.read = _Read_mpCalls_002Fc_GameOwner;
		Reader<mpCalls.c_GameIpSell>.read = _Read_mpCalls_002Fc_GameIpSell;
		Reader<mpCalls.c_GameIpPoints>.read = _Read_mpCalls_002Fc_GameIpPoints;
		Reader<mpCalls.c_GameSell>.read = _Read_mpCalls_002Fc_GameSell;
		Reader<mpCalls.c_Game>.read = _Read_mpCalls_002Fc_Game;
		Reader<mpCalls.s_AddPlayer>.read = _Read_mpCalls_002Fs_AddPlayer;
		Reader<mpCalls.s_Forschung>.read = _Read_mpCalls_002Fs_Forschung;
		Reader<mpCalls.s_PlayerLeave>.read = _Read_mpCalls_002Fs_PlayerLeave;
		Reader<mpCalls.s_GenreBeliebtheit>.read = _Read_mpCalls_002Fs_GenreBeliebtheit;
		Reader<float[]>.read = _Read_System_002ESingle_005B_005D;
		Reader<mpCalls.s_GenreCombination>.read = _Read_mpCalls_002Fs_GenreCombination;
		Reader<mpCalls.s_GenreDesign>.read = _Read_mpCalls_002Fs_GenreDesign;
		Reader<mpCalls.s_GenrePlatformSuit>.read = _Read_mpCalls_002Fs_GenrePlatformSuit;
		Reader<mpCalls.s_Help>.read = _Read_mpCalls_002Fs_Help;
		Reader<mpCalls.s_ObjectDelete>.read = _Read_mpCalls_002Fs_ObjectDelete;
		Reader<mpCalls.s_Object>.read = _Read_mpCalls_002Fs_Object;
		Reader<mpCalls.s_Map>.read = _Read_mpCalls_002Fs_Map;
		Reader<mpCalls.s_Office>.read = _Read_mpCalls_002Fs_Office;
		Reader<mpCalls.s_Difficulty>.read = _Read_mpCalls_002Fs_Difficulty;
		Reader<mpCalls.s_RandomSettings>.read = _Read_mpCalls_002Fs_RandomSettings;
		Reader<mpCalls.s_Wettbewerb>.read = _Read_mpCalls_002Fs_Wettbewerb;
		Reader<mpCalls.s_Startjahr>.read = _Read_mpCalls_002Fs_Startjahr;
		Reader<mpCalls.s_Entwicklungsdauer>.read = _Read_mpCalls_002Fs_Entwicklungsdauer;
		Reader<mpCalls.s_AnzahlKonkurrenten>.read = _Read_mpCalls_002Fs_AnzahlKonkurrenten;
		Reader<mpCalls.s_Spielgeschwindigkeit>.read = _Read_mpCalls_002Fs_Spielgeschwindigkeit;
		Reader<mpCalls.s_GlobalEvent>.read = _Read_mpCalls_002Fs_GlobalEvent;
		Reader<mpCalls.s_EngineAbrechnung>.read = _Read_mpCalls_002Fs_EngineAbrechnung;
		Reader<mpCalls.s_Awards>.read = _Read_mpCalls_002Fs_Awards;
		Reader<mpCalls.s_Payment>.read = _Read_mpCalls_002Fs_Payment;
		Reader<mpCalls.s_Engine>.read = _Read_mpCalls_002Fs_Engine;
		Reader<mpCalls.s_EnginePublisherBuyed>.read = _Read_mpCalls_002Fs_EnginePublisherBuyed;
		Reader<mpCalls.s_Platform>.read = _Read_mpCalls_002Fs_Platform;
		Reader<mpCalls.s_PlatformData>.read = _Read_mpCalls_002Fs_PlatformData;
		Reader<mpCalls.s_PlatformRemoveFromMarket>.read = _Read_mpCalls_002Fs_PlatformRemoveFromMarket;
		Reader<mpCalls.s_PlatformSubvention>.read = _Read_mpCalls_002Fs_PlatformSubvention;
		Reader<mpCalls.s_Chat>.read = _Read_mpCalls_002Fs_Chat;
		Reader<mpCalls.s_Money>.read = _Read_mpCalls_002Fs_Money;
		Reader<mpCalls.s_AutoPause>.read = _Read_mpCalls_002Fs_AutoPause;
		Reader<mpCalls.s_Genres>.read = _Read_mpCalls_002Fs_Genres;
		Reader<string[]>.read = _Read_System_002EString_005B_005D;
		Reader<mpCalls.s_Topics>.read = _Read_mpCalls_002Fs_Topics;
		Reader<mpCalls.s_GameplayFeatures>.read = _Read_mpCalls_002Fs_GameplayFeatures;
		Reader<mpCalls.s_EngineFeatures>.read = _Read_mpCalls_002Fs_EngineFeatures;
		Reader<mpCalls.s_HardwareFeatures>.read = _Read_mpCalls_002Fs_HardwareFeatures;
		Reader<mpCalls.s_Hardware>.read = _Read_mpCalls_002Fs_Hardware;
		Reader<mpCalls.s_AntiCheat>.read = _Read_mpCalls_002Fs_AntiCheat;
		Reader<mpCalls.s_CopyProtect>.read = _Read_mpCalls_002Fs_CopyProtect;
		Reader<mpCalls.s_NpcEngine>.read = _Read_mpCalls_002Fs_NpcEngine;
		Reader<mpCalls.s_TochterfirmaUmsatz>.read = _Read_mpCalls_002Fs_TochterfirmaUmsatz;
		Reader<mpCalls.s_Firmenwert>.read = _Read_mpCalls_002Fs_Firmenwert;
		Reader<long[]>.read = _Read_System_002EInt64_005B_005D;
		Reader<mpCalls.s_NpcGameName>.read = _Read_mpCalls_002Fs_NpcGameName;
		Reader<mpCalls.s_BlockContractGame>.read = _Read_mpCalls_002Fs_BlockContractGame;
		Reader<mpCalls.s_Publisher>.read = _Read_mpCalls_002Fs_Publisher;
		Reader<mpCalls.s_PublisherOwner>.read = _Read_mpCalls_002Fs_PublisherOwner;
		Reader<mpCalls.s_PublisherClose>.read = _Read_mpCalls_002Fs_PublisherClose;
		Reader<mpCalls.s_PublisherTochterfirmaSettings>.read = _Read_mpCalls_002Fs_PublisherTochterfirmaSettings;
		Reader<mpCalls.s_exklusivKonsolenSells>.read = _Read_mpCalls_002Fs_exklusivKonsolenSells;
		Reader<mpCalls.s_GameAnkuendigung>.read = _Read_mpCalls_002Fs_GameAnkuendigung;
		Reader<mpCalls.s_GameDestroy>.read = _Read_mpCalls_002Fs_GameDestroy;
		Reader<mpCalls.s_GameRemoveFromMarket>.read = _Read_mpCalls_002Fs_GameRemoveFromMarket;
		Reader<mpCalls.s_GameOwner>.read = _Read_mpCalls_002Fs_GameOwner;
		Reader<mpCalls.s_GameIpSell>.read = _Read_mpCalls_002Fs_GameIpSell;
		Reader<mpCalls.s_GameIpPoints>.read = _Read_mpCalls_002Fs_GameIpPoints;
		Reader<mpCalls.s_GameSell>.read = _Read_mpCalls_002Fs_GameSell;
		Reader<mpCalls.s_Game>.read = _Read_mpCalls_002Fs_Game;
		Reader<mpCalls.s_Lizenz>.read = _Read_mpCalls_002Fs_Lizenz;
		Reader<mpCalls.s_Trend>.read = _Read_mpCalls_002Fs_Trend;
		Reader<mpCalls.s_GameSpeed>.read = _Read_mpCalls_002Fs_GameSpeed;
		Reader<mpCalls.s_Command>.read = _Read_mpCalls_002Fs_Command;
		Reader<mpCalls.s_Save>.read = _Read_mpCalls_002Fs_Save;
		Reader<mpCalls.s_Load>.read = _Read_mpCalls_002Fs_Load;
		Reader<mpCalls.s_PlayerID>.read = _Read_mpCalls_002Fs_PlayerID;
		Reader<mpCalls.s_PlayerInfos>.read = _Read_mpCalls_002Fs_PlayerInfos;
		Reader<mpCalls.s_KillAA>.read = _Read_mpCalls_002Fs_KillAA;
		Reader<mpCalls.s_CreateArbeitsmarkt>.read = _Read_mpCalls_002Fs_CreateArbeitsmarkt;
	}
}
