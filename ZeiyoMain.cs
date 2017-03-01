using GTANetworkServer;
using GTANetworkShared;

namespace ZeiyoRP
{
    public class ZeiyoMain : Script
    {
       
        public ZeiyoMain()
        {
            API.setGamemodeName("zeiyorp");
            API.onResourceStart += myResourceStart;
        }

        public void myResourceStart()
        {
            API.consoleOutput("Starting Zeiyo !");
        }

        [Command("tpc")]
        public void Teleportation(Client sender, double x, double y, double z)
        {
            sender.position = new Vector3(x, y, z);
        }

        [Command("me", GreedyArg = true, AddToHelpmanager = true)]
        public void describe(Client sender, string text)
        {
            var msg = "* " + sender.name + " " + text;
            var players = API.getPlayersInRadiusOfPlayer(30, sender);

            foreach (Client c in players)
            {
                API.sendChatMessageToPlayer(c, msg);
            }
        }

    }

}

