using System;
using GTANetworkServer;
using GTANetworkShared;
using System.Threading;

namespace ZeiyoRP
{
    class ZFuel : Script
    {
        Thread fuelRefresh;
        NetHandle vehicle;
        Vector3[] fuelCoords = new Vector3[23];
        Blip[] fuelBlip = new Blip[23];

        public ZFuel(NetHandle v)
        {
            this.vehicle = v;
            this.fuelStart(v);
        }

        public ZFuel()
        {
            API.onResourceStart += serverStart;
            API.onEntityEnterColShape += isPlayerNearStation;
            API.onEntityExitColShape += isPlayerFarFromStation;
            API.onClientEventTrigger += addFuel;
        }

        /* Events */

        public void addFuel(Client sender, string eventName, object[] arguments)
        {

            if(eventName.Equals("addFuel"))
            {
                NetHandle vehicle = API.getPlayerVehicle(sender);
                int addFuel = (int)arguments[0];
                double fuel = API.getEntitySyncedData(vehicle, "fuel");
                double overFuel = (fuel + addFuel) - 50.0D ;
                bool over =  (fuel + addFuel) > 50.0D ? true : false;
                addFuel = over ? (int)(50.0D - fuel) : addFuel;
                API.freezePlayer(sender, true);

                while (API.getEntitySyncedData(vehicle, "fuel") < fuel + addFuel)
                {
                    API.setEntitySyncedData(vehicle, "fuel", (API.getEntitySyncedData(vehicle, "fuel") + 0.4));
                    System.Threading.Thread.Sleep(100);
                }

                API.freezePlayer(sender, false);
                string overText = over ? ( "Vous avez demandé " + overFuel + " litres en trop, le plein de votre vehicule a cependant été effectué : \n" ) : "";
                string text = "Vous avez versé : " + addFuel + " litres dans votre reservoir, ce qui vous a couté $";
                API.sendChatMessageToPlayer(sender, overText + text);
            }

        }

        public void isPlayerFarFromStation(ColShape colshape, NetHandle entity)
        {
            var player = API.getPlayerFromHandle(entity);

            if (player == null) { return; }

            else if (colshape.hasData("fuelStation"))
            {
                API.triggerClientEvent(player, "NfuelStation");
            }
        }

        public void isPlayerNearStation(ColShape colshape, NetHandle entity)
        {
            var player = API.getPlayerFromHandle(entity);

            if(player == null) { return;}

            else if (colshape.hasData("fuelStation") && !API.getPlayerVehicle(player).IsNull && API.hasEntitySyncedData(API.getPlayerVehicle(player), "fuel"))
            {            
                API.triggerClientEvent(player, "fuelStation");
            }
           
        }

        public void serverStart()
        {       
            getFuelStationsCoords();
        }
       
        /* Initialisateur du Thread */

        public void fuelStart(NetHandle vehicle)
        {

            if (API.hasEntitySyncedData(vehicle, "fuel"))
                {
                    fuelRefresh = new Thread(new ThreadStart(ThreadFuel));
                    fuelRefresh.Start();
                }

                else
                {
                    fuelRefresh = new Thread(new ThreadStart(ThreadFuel));
                    API.setEntitySyncedData(vehicle, "fuel", getMaxFuel(vehicle));
                    fuelRefresh.Start();
                }
            }

        /* Thread qui calcule la consommation de l'essence d'un vehicule */

        private void ThreadFuel()
        {

            while (API.getVehicleEngineStatus(vehicle) && API.getVehicleHealth(vehicle) > 0)
            {
                if (API.getEntitySyncedData(vehicle, "fuel") < 0.49)
                {
                    API.setVehicleEngineStatus(vehicle, false);
                    break;
                }

                else
                {
                    while (API.getVehicleOccupants(vehicle).Length > 0 && API.getVehicleEngineStatus(vehicle) && API.getEntitySyncedData(vehicle, "fuel") > 0)
                    {
                        if (API.getEntitySyncedData(vehicle, "fuel") < 0.49)
                        {
                            break;
                        }

                        else
                        {
                            API.triggerClientEvent(API.getVehicleOccupants(vehicle)[0], "fuelRef", vehicle);
                            System.Threading.Thread.Sleep(250);
                        }
                    }

                    API.setEntitySyncedData(vehicle, "fuel", (API.getEntitySyncedData(vehicle, "fuel") - 0.002));
                    System.Threading.Thread.Sleep(250);

                }
            }

            fuelRefresh.Abort();
        }

        /* Getters */

        public void getFuelStationsCoords()
        {
            string[] lines = System.IO.File.ReadAllLines("resources/zeiyorp/fuelstations.txt");
            int a = 0;

            foreach (string line in lines)
            {
                char[] separator = new char[1];
                separator[0] = ',';
                string[] words = line.Split(separator);
                fuelCoords[a] = new Vector3(Convert.ToSingle(words[0]), Convert.ToSingle(words[1]), Convert.ToSingle(words[2]));
                fuelBlip[a] = API.createBlip(fuelCoords[a]);
                API.setBlipName(fuelBlip[a], "Station-Essence");
                API.setBlipSprite(fuelBlip[a], 361);
                API.setBlipScale(fuelBlip[a], 0.7F);
                API.setBlipShortRange(fuelBlip[a], true);
                var fuelStation = API.createSphereColShape(fuelCoords[a], 9.0F);
                fuelStation.setData("fuelStation", true);
                fuelStation.setData("fuelVol", 1400.0F);

                a++;
            }
        }

        public double getMaxFuel(NetHandle vehicle)
        {

                var hashNmbr = (VehicleHash)API.getEntityModel(vehicle);
                double maxCapacity;

                switch (API.getVehicleClass(hashNmbr))
                {
                    case 0:
                        maxCapacity = 34;
                        break;
                    case 1:
                        maxCapacity = 50;
                        break;
                    case 2:
                        maxCapacity = 50;
                        break;
                    case 3:
                        maxCapacity = 50;
                        break;
                    case 4:
                        maxCapacity = 50;
                        break;
                    case 5:
                        maxCapacity = 50;
                        break;
                    case 6:
                        maxCapacity = 50;
                        break;
                    case 7:
                        maxCapacity = 50;
                        break;
                    case 8:
                        maxCapacity = 50;
                        break;
                    case 9:
                        maxCapacity = 50;
                        break;
                    case 10:
                        maxCapacity = 50;
                        break;
                    case 11:
                        maxCapacity = 50;
                        break;
                    case 12:
                        maxCapacity = 50;
                        break;
                    case 13:
                        maxCapacity = 50;
                        break;
                    case 14:
                        maxCapacity = 90;
                        break;
                    case 15:
                        maxCapacity = 50;
                        break;
                    case 16:
                        maxCapacity = 50;
                        break;
                    case 17:
                        maxCapacity = 50;
                        break;
                    case 18:
                        maxCapacity = 50;
                        break;
                    case 19:
                        maxCapacity = 50;
                        break;
                    case 20:
                        maxCapacity = 160;
                        break;
                    case 21:
                        maxCapacity = 50;
                        break;

                    default:
                        maxCapacity = 50;
                        break;
                }
       
            return maxCapacity;
        }

    }
}


   