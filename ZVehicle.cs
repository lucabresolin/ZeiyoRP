using GTANetworkServer;
using GTANetworkShared;

namespace ZeiyoRP
{
    public class ZVehicle : Script
    {
        ZFuel fuel;
        ZeiyoMain main = new ZeiyoMain();

        public ZVehicle()
        {
            API.onClientEventTrigger += clientVehiclesEvent;
            API.onPlayerEnterVehicle += enterVehicle;
        }
     
        /* Events */
      
        public void clientVehiclesEvent(Client player, string eventName, params object[] arguments)
        {

            NetHandle vehicle = API.getPlayerVehicle(player);

            if (eventName.Equals("engine"))
            {
                turnEngine(player);
            }

            else if(eventName.Equals("hood"))
            {

                if(!API.getVehicleDoorState(vehicle, 4))
                {
                    openCarDoors(player, "hood");
                }

                else
                {
                    closeCarDoors(player, "hood");
                }

            }

            else if (eventName.Equals("trunk"))
            {

                if (!API.getVehicleDoorState(vehicle, 5))
                {
                    openCarDoors(player, "trunk");
                }

                else
                {
                    closeCarDoors(player, "trunk");
                }

            }

            else if (eventName.Equals("seatbelt"))
            {               
                  putSeatbelt(player);        
            }

        }

        public void enterVehicle(Client player, NetHandle vehicle)
        {
            engineTweak(player, vehicle);
        }


        /* Fonctions */

        public void engineTweak(Client player, NetHandle vehicle)
        {
            if (!API.getPlayerVehicle(player).IsNull && API.getPlayerVehicleSeat(player) == -1 && !API.getVehicleEngineStatus(API.getPlayerVehicle(player)))
            {
                API.setVehicleEngineStatus(vehicle, false);
            }

            else
            {
                return;
            }
        }

        public void turnEngine(Client player)
        {

            NetHandle vehicle = API.getPlayerVehicle(player);

            if (!vehicle.IsNull)
            {

                if (API.getPlayerVehicleSeat(player) == -1)
                {
                    if (!API.getVehicleEngineStatus(vehicle) && API.getEntitySyncedData(vehicle, "fuel") < 0.49)
                    {
                        player.freeze(true);
                        main.describe(player, "essaie d'allumer le moteur");
                        System.Threading.Thread.Sleep(750);
                        player.freeze(false);
                        API.sendChatMessageToPlayer(player, "Il n'y a plus d'essence !");
                    }

                    else if (!API.getVehicleEngineStatus(vehicle))
                    {
                        player.freeze(true);
                        main.describe(player, "est en train de demarrer");
                        System.Threading.Thread.Sleep(1000);
                        player.freeze(false);
                        API.setVehicleEngineStatus(vehicle, true);
                        fuel = new ZFuel(vehicle);
                    }

                    else
                    {
                        player.freeze(true);
                        main.describe(player, "coupe le moteur de son vehicule");
                        System.Threading.Thread.Sleep(750);
                        player.freeze(false);
                        API.setVehicleEngineStatus(vehicle, false);

                    }
                }

                else
                {
                    API.sendChatMessageToPlayer(player, "Vous n'etes pas le conducteur du vehicule");
                    return;
                }

            }

            else
            {
                API.sendChatMessageToPlayer(player, "Vous n'etes pas dans un vehicule");
                return;
            }

        }

        public void putSeatbelt(Client player)
        {

            NetHandle vehicle = API.getPlayerVehicle(player);

            if (!vehicle.IsNull)
            {

                if (API.getPlayerSeatbelt(player))
                {
                    player.freeze(true);
                    main.describe(player, "enlève sa ceinture de sécurité");
                    System.Threading.Thread.Sleep(1000);
                    player.freeze(false);
                    API.setPlayerSeatbelt(player, false);
                }

                else
                {
                    player.freeze(true);
                    main.describe(player, "met sa ceinture de sécurité");
                    System.Threading.Thread.Sleep(1000);
                    player.freeze(false);
                    API.setPlayerSeatbelt(player, true);
                }

            }

            else
            {
                API.sendChatMessageToPlayer(player, "Vous n'etes pas dans un vehicule");
                return;
            }

        }

        /* Commandes */

        [Command("open")]
        public void openCarDoors(Client player, string part)
        {

            NetHandle vehicle = API.getPlayerVehicle(player);

            if (!vehicle.IsNull)
            {
                if (API.getPlayerVehicleSeat(player) == -1 && API.getVehicleEngineStatus(vehicle))
                {
                    if (part.Equals("trunk"))
                    {
                        player.freeze(true);
                        System.Threading.Thread.Sleep(400);
                        player.freeze(false);
                        API.sendNativeToAllPlayers(0x7C65DAC73C35C862, vehicle, 5, true, true);
                    }

                    else if (part.Equals("hood"))
                    {
                        player.freeze(true);
                        System.Threading.Thread.Sleep(400);
                        player.freeze(false);
                        API.sendNativeToAllPlayers(0x7C65DAC73C35C862, vehicle, 4, true, true);
                    }

                    else if (part.Equals("prdoor"))
                    {
                        player.freeze(true);
                        System.Threading.Thread.Sleep(400);
                        player.freeze(false);
                        API.sendNativeToAllPlayers(0x7C65DAC73C35C862, vehicle, 3, true, true);
                    }

                    else if (part.Equals("drdoor"))
                    {
                        player.freeze(true);
                        System.Threading.Thread.Sleep(400);
                        player.freeze(false);
                        API.sendNativeToAllPlayers(0x7C65DAC73C35C862, vehicle, 2, true, true);
                    }

                    else if (part.Equals("pdoor"))
                    {
                        player.freeze(true);
                        System.Threading.Thread.Sleep(400);
                        player.freeze(false);
                        API.sendNativeToAllPlayers(0x7C65DAC73C35C862, vehicle, 1, true, true);
                    }

                    else if (part.Equals("drdoor"))
                    {
                        player.freeze(true);
                        System.Threading.Thread.Sleep(400);
                        player.freeze(false);
                        API.sendNativeToAllPlayers(0x7C65DAC73C35C862, vehicle, 0, true, true);
                    }

                    else if (part.Equals("all"))
                    {
                        player.freeze(true);
                        System.Threading.Thread.Sleep(400);
                        player.freeze(false);
                        API.sendNativeToAllPlayers(0x7C65DAC73C35C862, vehicle, 0, true, true);
                        API.sendNativeToAllPlayers(0x7C65DAC73C35C862, vehicle, 1, true, true);
                        API.sendNativeToAllPlayers(0x7C65DAC73C35C862, vehicle, 2, true, true);
                        API.sendNativeToAllPlayers(0x7C65DAC73C35C862, vehicle, 3, true, true);
                        API.sendNativeToAllPlayers(0x7C65DAC73C35C862, vehicle, 4, true, true);
                        API.sendNativeToAllPlayers(0x7C65DAC73C35C862, vehicle, 5, true, true);
                    }

                    else
                    {
                        API.sendChatMessageToPlayer(player, ("\"" + part + "\"" + " ne correspond a aucune partie du vehicule"));
                        return;
                    }
                }

                else
                {
                    API.sendChatMessageToPlayer(player, ("Vous ne pouvez pas ouvrir de portes"));
                    return;
                }
            }

            else
            {
                API.sendChatMessageToPlayer(player, ("Vous ne n'etes pas dans un vehicule"));
                return;
            }
        }

        [Command("close")]
        public void closeCarDoors(Client player, string part)
        {

            NetHandle vehicle = API.getPlayerVehicle(player);

            if (!vehicle.IsNull)
            {
                if (API.getPlayerVehicleSeat(player) == -1 && API.getVehicleEngineStatus(vehicle))
                {
                    if (part.Equals("trunk"))
                    {
                        player.freeze(true);
                        System.Threading.Thread.Sleep(400);
                        player.freeze(false);
                        API.sendNativeToAllPlayers(0x93D9BD300D7789E5, vehicle, 5, true, true);
                    }

                    else if (part.Equals("hood"))
                    {
                        player.freeze(true);
                        System.Threading.Thread.Sleep(400);
                        player.freeze(false);
                        API.sendNativeToAllPlayers(0x93D9BD300D7789E5, vehicle, 4, true, true);
                    }

                    else if (part.Equals("prdoor"))
                    {
                        player.freeze(true);
                        System.Threading.Thread.Sleep(400);
                        player.freeze(false);
                        API.sendNativeToAllPlayers(0x93D9BD300D7789E5, vehicle, 3, true, true);
                    }

                    else if (part.Equals("drdoor"))
                    {
                        player.freeze(true);
                        System.Threading.Thread.Sleep(400);
                        player.freeze(false);
                        API.sendNativeToAllPlayers(0x93D9BD300D7789E5, vehicle, 2, true, true);
                    }

                    else if (part.Equals("pdoor"))
                    {
                        player.freeze(true);
                        System.Threading.Thread.Sleep(400);
                        player.freeze(false);
                        API.sendNativeToAllPlayers(0x93D9BD300D7789E5, vehicle, 1, true, true);
                    }

                    else if (part.Equals("drdoor"))
                    {
                        player.freeze(true);
                        System.Threading.Thread.Sleep(400);
                        player.freeze(false);
                        API.sendNativeToAllPlayers(0x93D9BD300D7789E5, vehicle, 0, true, true);
                    }

                    else if (part.Equals("all"))
                    {
                        player.freeze(true);
                        System.Threading.Thread.Sleep(400);
                        player.freeze(false);
                        API.sendNativeToAllPlayers(0x93D9BD300D7789E5, vehicle, 0, true, true);
                        API.sendNativeToAllPlayers(0x93D9BD300D7789E5, vehicle, 1, true, true);
                        API.sendNativeToAllPlayers(0x93D9BD300D7789E5, vehicle, 2, true, true);
                        API.sendNativeToAllPlayers(0x93D9BD300D7789E5, vehicle, 3, true, true);
                        API.sendNativeToAllPlayers(0x93D9BD300D7789E5, vehicle, 4, true, true);
                        API.sendNativeToAllPlayers(0x93D9BD300D7789E5, vehicle, 5, true, true);
                    }

                    else
                    {
                        API.sendChatMessageToPlayer(player, ("\"" + part + "\"" + " ne correspond a aucune partie du vehicule"));
                        return;
                    }
                }

                else
                {
                    API.sendChatMessageToPlayer(player, ("Vous ne pouvez pas fermer de portes"));
                    return;
                }
            }

            else
            {
                API.sendChatMessageToPlayer(player, ("Vous n'etes pas dans un vehicule"));
                return;
            }

        }

        [Command("lock")]
        public void lockVehicle(Client sender)
        {

            NetHandle vehicle = API.getPlayerVehicle(sender);

            if(!vehicle.IsNull)
            {
                if (API.getPlayerVehicleSeat(sender) == -1 && !API.getVehicleLocked(vehicle))
                {
                    API.setVehicleLocked(vehicle, true);
                }

                else
                {
                    API.sendChatMessageToPlayer(sender, "Vous ne pouvez pas verouiller le vehicule");
                    return;
                }
                
            }

            else
            {
                API.sendChatMessageToPlayer(sender, "Vous n'etes pas dans un vehicule");
                return; 
            }
                
        }

        [Command("unlock")]
        public void unlockVehicle(Client sender)
        {

            NetHandle vehicle = API.getPlayerVehicle(sender);

            if (!vehicle.IsNull)
            {

                if (API.getPlayerVehicleSeat(sender) == -1 && API.getVehicleLocked(vehicle))
                {

                    API.setVehicleLocked(vehicle, false);

                }

                else
                {
                    API.sendChatMessageToPlayer(sender, "Vous ne pouvez pas deverouiller le vehicule");
                    return;
                }

            }

            else
            {
                API.sendChatMessageToPlayer(sender, "Vous n'etes pas dans un vehicule");
                return;
            }

        }
      
        [Command("setVehicleColor")]
        public void setVehicleColor(Client sender, int color)
        {

            if (API.getPlayerVehicle(sender).GetHashCode() != 0 && API.getPlayerVehicleSeat(sender) == -1)
            {

                NetHandle vehicle = API.getPlayerVehicle(sender);
                API.setVehiclePrimaryColor(vehicle, color);
                API.setVehicleSecondaryColor(vehicle, color);

            }

        }

        [Command("repair")]
        public void repairVehicle(Client sender)
        {

            NetHandle vehicle = API.getPlayerVehicle(sender);

            if (!vehicle.IsNull)
            {
                API.repairVehicle(vehicle);
            }

            else
            {
                API.sendChatMessageToPlayer(sender, "Vous n'etes pas dans un vehicule");
                return;
            }

        }

        [Command("setFuel")]
        public void setFuel(Client sender, double amount)
        {

            NetHandle vehicle = API.getPlayerVehicle(sender);

            if (!vehicle.IsNull)
            {
                API.setEntitySyncedData(vehicle, "fuel", amount);
            }

            else
            {
                API.sendChatMessageToPlayer(sender, "Vous n'etes pas dans un vehicule");
                return;
            }

        }

    }
}


