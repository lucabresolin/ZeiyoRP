/// <reference path="types-gtanetwork/index.d.ts" />

var menuPool = null;
var player = API.getLocalPlayer();
var previousRPM = 0;
var resY = API.getScreenResolution().Height;
var resX = API.getScreenResolution().Width;
var fuelCoords;
var canFuel = false;
var fuelMenu;
var isMapOpen = false;

	/* Interface des vehicules et de la station-essence */

	API.onKeyDown.connect(function (Player, args)
    {
        if (args.KeyCode == Keys.O && !API.isChatOpen() && !API.getPlayerVehicle(player).IsNull) {
            var vehicle = API.getPlayerVehicle(player);

            // Verifie que le modele est une voiture
            if (API.returnNative("0x7F6DB52EEFC96DF8", 8, API.getEntityModel(vehicle))) {

                menuPool = API.getMenuPool();
                var vehicleMenu = API.createMenu("Vehicule", "Options du vehicule", 0, 0, 6);
                var itemEngine = API.createMenuItem("Allumer/Eteindre le moteur", "")
                var itemHood = API.createMenuItem("Ouvrir/Fermer le capot", "");
                var itemTrunk = API.createMenuItem("Ouvrir/Fermer le coffre", "");
                var itemSeatbelt = API.createMenuItem("Mettre/Enlever la ceinture de securite", "");

                vehicleMenu.AddItem(itemEngine);
                vehicleMenu.AddItem(itemHood);
                vehicleMenu.AddItem(itemTrunk);
                vehicleMenu.AddItem(itemSeatbelt);

                menuPool.Add(vehicleMenu);
                vehicleMenu.Visible = true;

                itemEngine.Activated.connect(function (menu, item) {
                    API.triggerServerEvent("engine");
                });

                itemHood.Activated.connect(function (menu, item) {
                    API.triggerServerEvent("hood");
                });

                itemTrunk.Activated.connect(function (menu, item) {
                    API.triggerServerEvent("trunk");
                });

                itemSeatbelt.Activated.connect(function (menu, item) {
                    API.triggerServerEvent("seatbelt");
                });
            }

            // Verifie que le modele est une moto ou un quad
            else if (API.returnNative("0xB50C0B0CEDC6CE84 ", 8, API.getEntityModel(vehicle) || API.returnNative("0x39DAC362EE65FA28", 8, API.getEntityModel(vehicle)))) {

                menuPool = API.getMenuPool();
                var vehicleMenu = API.createMenu("Vehicule", "Options du vehicule", 0, 0, 6);
                var itemEngine = API.createMenuItem("Allumer/Eteindre le moteur", "")

                vehicleMenu.AddItem(itemEngine);
                menuPool.Add(vehicleMenu);
                vehicleMenu.Visible = true;

                itemEngine.Activated.connect(function (menu, item) {
                    API.triggerServerEvent("engine");
                });
            }

            // Verifie que le modele est un avion
            else if (API.returnNative("0xA0948AB42D7BA0DE ", 8, API.getEntityModel(vehicle))) {

                menuPool = API.getMenuPool();
                var vehicleMenu = API.createMenu("Vehicule", "Options du vehicule", 0, 0, 6);
                var itemEngine = API.createMenuItem("Allumer/Eteindre le moteur", "");
                var itemSeatbelt = API.createMenuItem("Mettre/Enlever la ceinture de securite", "");

                vehicleMenu.AddItem(itemEngine);
                vehicleMenu.AddItem(itemSeatbelt);
                menuPool.Add(vehicleMenu);
                vehicleMenu.Visible = true;

                itemEngine.Activated.connect(function (menu, item) {
                    API.triggerServerEvent("engine");
                });

                itemSeatbelt.Activated.connect(function (menu, item) {
                    API.triggerServerEvent("seatbelt");
                });
            }

            // Verifie que le modele est un helicoptere
            else if (API.returnNative("0xDCE4334788AF94EA", 8, API.getEntityModel(vehicle))) {

                menuPool = API.getMenuPool();
                var vehicleMenu = API.createMenu("Vehicule", "Options du vehicule", 0, 0, 6);
                var itemEngine = API.createMenuItem("Allumer/Eteindre le moteur", "")
                var itemSeatbelt = API.createMenuItem("Mettre/Enlever la ceinture de securite", "");

                vehicleMenu.AddItem(itemEngine);
                vehicleMenu.AddItem(itemSeatbelt);
                menuPool.Add(vehicleMenu);
                vehicleMenu.Visible = true;

                itemEngine.Activated.connect(function (menu, item) {
                    API.triggerServerEvent("engine");
                });

                itemSeatbelt.Activated.connect(function (menu, item) {
                    API.triggerServerEvent("seatbelt");
                });
            }

            // Verifie que le vehicule est un bateau
            else if (API.returnNative("0x45A9187928F4B9E3", 8, API.getEntityModel(vehicle))) {

                menuPool = API.getMenuPool();
                var vehicleMenu = API.createMenu("Vehicule", "Options du vehicule", 0, 0, 6);
                var itemEngine = API.createMenuItem("Allumer/Eteindre le moteur", "")

                vehicleMenu.AddItem(itemEngine);
                menuPool.Add(vehicleMenu);
                vehicleMenu.Visible = true;

                itemEngine.Activated.connect(function (menu, item) {
                    API.triggerServerEvent("engine");
                });
            }

            else {

                menuPool = API.getMenuPool();
                var vehicleMenu = API.createMenu("Vehicule", "Options du vehicule", 0, 0, 6);
                var itemEngine = API.createMenuItem("Allumer/Eteindre le moteur", "")

                vehicleMenu.AddItem(itemEngine);
                menuPool.Add(vehicleMenu);
                vehicleMenu.Visible = true;

                itemEngine.Activated.connect(function (menu, item) {
                    API.triggerServerEvent("engine");
                });

            }
        }

        if (args.KeyCode == Keys.K && !API.isChatOpen() && canFuel && !API.getPlayerVehicle(player).IsNull)
        {
            menuPool = API.getMenuPool();
            fuelMenu = API.createMenu("Station-Essence", "Vous pouvez faire le plein !", 0, 0, 6);
            var itemPrice = API.createMenuItem("Prix : " + " $1/l", "");
            var itemFuel = API.createMenuItem("Ajouter de l'essence", "Choisir le nombre de litres à verser");

            fuelMenu.AddItem(itemPrice);
            fuelMenu.AddItem(itemFuel);

            menuPool.Add(fuelMenu);
            fuelMenu.Visible = true;

            itemFuel.Activated.connect(function (menu, item) {
                do {
                    var amount = API.getUserInput("", 3);
                } while (Number(amount) > 50 || Number(amount) < 0 || isNaN(Number(amount)));
                API.triggerServerEvent("addFuel", Number(amount));
                fuelMenu.Visible = false;
            });
        }
    });

    /* Calcul de la consommation de l'essence */

    API.onServerEventTrigger.connect(function (eventName, args) {
        switch (eventName) {

            case 'fuelRef':

                var vehicle = args[0];
                var fuel = API.getEntitySyncedData(vehicle, "fuel");
                var RPM = API.getVehicleRPM(vehicle);

                var conso = (7/1000) * (RPM + 4 * (Math.abs(RPM - previousRPM)));
                API.setEntitySyncedData(vehicle, "fuel", (fuel - conso));                
                previousRPM = RPM;

                break;

            case 'fuelStation':
                canFuel = true;
                API.sendNotification("Tu peux faire le plein avec la touche 'K' ");

                break;

            case 'NfuelStation':
                canFuel = false;
                break;
        }
    });

    /* Interfaces */
    
    API.onUpdate.connect(function () {

        if (!API.getPlayerVehicle(player).IsNull && API.getPlayerVehicleSeat(player) == -1) 
        {
            API.callNative("0x3A618A217E5154F0", 0.09, 0.804, 0.165, 0.019, 0, 0, 0, 126);

            if (API.getEntitySyncedData(API.getPlayerVehicle(player), "fuel") < 12.25)
            {
                API.callNative("0x3A618A217E5154F0", 0.09, 0.805, 0.16, 0.0085, 220, 20, 20, 60);
                API.callNative("0x3A618A217E5154F0", 0.0102 + ((API.getEntitySyncedData(API.getPlayerVehicle(player), "fuel") * (0.16 / 50)) / 2), 0.805, (API.getEntitySyncedData(API.getPlayerVehicle(player), "fuel") * (0.16 / 50)), 0.0085, 220, 20 ,20, 210);
            }

            else
            {
                API.callNative("0x3A618A217E5154F0", 0.09, 0.805, 0.16, 0.0085, 255, 140, 73, 60);
                API.callNative("0x3A618A217E5154F0", 0.0102 + ((API.getEntitySyncedData(API.getPlayerVehicle(player), "fuel") * (0.16 / 50)) / 2), 0.805, (API.getEntitySyncedData(API.getPlayerVehicle(player), "fuel") * (0.16 / 50)), 0.0085, 220, 127, 20, 210);
            }
          
        } 

		if (menuPool != null) {
			menuPool.ProcessMenus();
        }
    });

    /* Recupere la contenance maximale du reservoir d'un vehicule */

    function getVehicleMaxFuel(vehicle)
    {
        var hashNmbr = API.getEntityModel(vehicle);
        var maxCapacity;

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

    }
