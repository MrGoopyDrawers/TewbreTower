using MelonLoader;
using BTD_Mod_Helper;
using TewbreTower;
using BTD_Mod_Helper.Api;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models;
using Il2CppAssets.Scripts.Simulation.Objects;
using Il2CppAssets.Scripts.Simulation.Towers;
using Il2CppAssets.Scripts.Simulation.Towers.Weapons;
using UnityEngine;
using Random = System.Random;
using System.Collections.Generic;
using System.Linq;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.TowerSets;
using BTD_Mod_Helper.Api.Enums;
using BTD_Mod_Helper.Api.Towers;
using Il2CppAssets.Scripts.Unity;
using Il2CppAssets.Scripts.Unity.Display;
using BTD_Mod_Helper.Api.Display;
using Il2CppAssets.Scripts.Models.Towers.Behaviors;
using Il2CppAssets.Scripts.Models.GenericBehaviors;
using Il2CppAssets.Scripts.Simulation.SMath;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Abilities.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Abilities;
using Il2CppAssets.Scripts.Models.Towers.TowerFilters;
using Il2CppAssets.Scripts.Models.Map;
using Il2CppAssets.Scripts.Models.Audio;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Attack;
using Il2CppAssets.Scripts.Models.Towers.Weapons.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Emissions;
using Il2CppAssets.Scripts.Models.Towers.Filters;
using HarmonyLib;
using Il2Cpp;

[assembly: MelonInfo(typeof(TewbreTower.TewbreTower), ModHelperData.Name, ModHelperData.Version, ModHelperData.RepoOwner)]
[assembly: MelonGame("Ninja Kiwi", "BloonsTD6")]

namespace TewbreTower;

public class TewbreTower : BloonsTD6Mod
{
    public override void OnApplicationStart()
    {
        ModHelper.Msg<TewbreTower>("TewbreTower loaded!");
    }
    public override void OnTowerCreated(Tower tower, Entity target, Model modelToUse)
    {
        if (tower.model.name.Contains("tewbre"))
        {
            ModContent.GetAudioClip<TewbreTower>("PlaceTewbreSound").Play();
        }
    }
    public class tewbre : ModTower
    {


        public override TowerSet TowerSet => TowerSet.Primary;
        public override bool Use2DModel => true;
        public override string BaseTower => TowerType.DartMonkey;
        public override int Cost => 250;

        public override int TopPathUpgrades => 5;
        public override int MiddlePathUpgrades => 5;
        public override int BottomPathUpgrades => 5;
        public override string Description => "Hey guys! Welcome back to another TEWBRE video!";
        public override string DisplayName => "Tewbre";
        public override ParagonMode ParagonMode => ParagonMode.Base000;
        public override bool IsValidCrosspath(int[] tiers) => ModHelper.HasMod("Ultimate Crosspathing") ? true : base.IsValidCrosspath(tiers);
        public override void ModifyBaseTowerModel(TowerModel towerModel)
        {
            towerModel.range += 40;
            var attackModel = towerModel.GetAttackModel();
            attackModel.range += 40;
            attackModel.weapons[0].projectile.GetBehavior<TravelStraitModel>().Lifespan = 6.0f;
            attackModel.weapons[0].projectile.GetBehavior<TravelStraitModel>().Speed *= 0.5f;
            var placesound = Game.instance.model.GetTowerFromId("Sauda").GetBehavior<CreateSoundOnBloonEnterTrackModel>().Duplicate();
            placesound.moabSound.assetId = ModContent.CreateAudioSourceReference<TewbreTower>("TewbreMassiveDawg");
            placesound.bfbSound.assetId = ModContent.CreateAudioSourceReference<TewbreTower>("TewbreMassiveDawg");
            placesound.ddtSound.assetId = ModContent.CreateAudioSourceReference<TewbreTower>("TewbreMassiveDawg");
            placesound.zomgSound.assetId = ModContent.CreateAudioSourceReference<TewbreTower>("TewbreMassiveDawg");
            placesound.badSound.assetId = ModContent.CreateAudioSourceReference<TewbreTower>("TewbreMassiveDawg");
            var snipeSound = Game.instance.model.GetTowerFromId("Sauda").GetBehavior<CreateSoundOnBloonLeakModel>().Duplicate();
            snipeSound.sound1.assetId = ModContent.CreateAudioSourceReference<TewbreTower>("TewbreDefeat");
            snipeSound.sound2.assetId = ModContent.CreateAudioSourceReference<TewbreTower>("TewbreAlmostLost");
            var snipeSound2 = Game.instance.model.GetTowerFromId("Sauda").GetBehavior<CreateSoundOnSelectedModel>().Duplicate();
            snipeSound2.sound1.assetId = ModContent.CreateAudioSourceReference<TewbreTower>("TewbreSnipeSound1");
            snipeSound2.sound2.assetId = ModContent.CreateAudioSourceReference<TewbreTower>("TewbreSnipeSound2");
            snipeSound2.sound3.assetId = ModContent.CreateAudioSourceReference<TewbreTower>("TewbreSnipeSound2");
            snipeSound2.sound4.assetId = ModContent.CreateAudioSourceReference<TewbreTower>("TewbreSnipeSound1");
            snipeSound2.sound5.assetId = ModContent.CreateAudioSourceReference<TewbreTower>("TewbreSnipeSound1");
            snipeSound2.sound6.assetId = ModContent.CreateAudioSourceReference<TewbreTower>("TewbreSnipeSound2");
            snipeSound2.altSound1.assetId = ModContent.CreateAudioSourceReference<TewbreTower>("TewbreSnipeSound2");
            snipeSound2.altSound2.assetId = ModContent.CreateAudioSourceReference<TewbreTower>("TewbreSnipeSound2");
            towerModel.AddBehavior(placesound);
            towerModel.AddBehavior(snipeSound);
            towerModel.AddBehavior(snipeSound2);
            attackModel.weapons[0].rate *= 1.4f;
            attackModel.weapons[0].projectile.pierce += 8;
            attackModel.weapons[0].projectile.AddBehavior(Game.instance.model.GetTowerFromId("Adora 20").GetAttackModel().weapons[0].projectile.GetBehavior<AdoraTrackTargetModel>().Duplicate());
            var bananaFarmAttackModel = Game.instance.model.GetTowerFromId("BananaFarm-003").GetAttackModel().Duplicate();
            bananaFarmAttackModel.name = "Tewbre_";
            bananaFarmAttackModel.weapons[0].projectile.GetBehavior<CashModel>().maximum = 20;
            bananaFarmAttackModel.weapons[0].projectile.GetBehavior<CashModel>().minimum = 20;
            bananaFarmAttackModel.weapons[0].GetBehavior<EmissionsPerRoundFilterModel>().count = 5;
            towerModel.AddBehavior(bananaFarmAttackModel);

        }
        public override string Get2DTexture(int[] tiers)
        {
            return "TewbreMainImage";
        }
        public class marketplace : ModUpgrade<tewbre>
        {
            public override int Path => TOP;
            public override int Tier => 1;
            public override int Cost => 600;
            public override string Icon => "sabre";
            public override string DisplayName => "I NEED A MARKETPLACE TEWITY";

            public override string Description => "Donate to sabre and produce +5 bananas per round.";

            public override void ApplyUpgrade(TowerModel towerModel)
            {
                towerModel.GetAttackModel(1).weapons[0].GetBehavior<EmissionsPerRoundFilterModel>().count += 5;
            }
        }
        public class bananaplantation : ModUpgrade<tewbre>
        {
            public override int Path => TOP;
            public override int Tier => 2;
            public override int Cost => 750;
            public override string Icon => "sabre2";
            public override string DisplayName => "I NEED TO CROSSPATH IT TEWITY";

            public override string Description => "Donate to sabre and produce another 5 bananas.";

            public override void ApplyUpgrade(TowerModel towerModel)
            {
                towerModel.GetAttackModel(1).weapons[0].GetBehavior<EmissionsPerRoundFilterModel>().count += 5;
            }
        }
        public class bananarepublic : ModUpgrade<tewbre>
        {
            public override int Path => TOP;
            public override int Tier => 3;
            public override int Cost => 5000;
            public override string Icon => "sabre3";
            public override string DisplayName => "I NEED TO CROSSPATH IT WITH BANANA RESEARCH FACILITY TEWITY";

            public override string Description => "Donate to sabre and produce very valuabe bananas";

            public override void ApplyUpgrade(TowerModel towerModel)
            {
                towerModel.GetAttackModel(1).weapons[0].projectile.GetBehavior<CashModel>().maximum = 300;
                towerModel.GetAttackModel(1).weapons[0].projectile.GetBehavior<CashModel>().minimum = 300;
            }
        }
        public class bananaCentral : ModUpgrade<tewbre>
        {
            public override int Path => TOP;
            public override int Tier => 4;
            public override int Cost => 25000;
            public override string Icon => "sabre4";
            public override string DisplayName => "I NEED A BANANA CENTRAL TEWITY";

            public override string Description => "Donate to sabre and produce incredibly valuable bananas at x2 the power";

            public override void ApplyUpgrade(TowerModel towerModel)
            {
                towerModel.GetAttackModel(1).weapons[0].projectile.GetBehavior<CashModel>().maximum = 750;
                towerModel.GetAttackModel(1).weapons[0].projectile.GetBehavior<CashModel>().minimum = 750;
                towerModel.GetAttackModel(1).weapons[0].GetBehavior<EmissionsPerRoundFilterModel>().count += 10;
            }
        }
        public class richSabre : ModUpgrade<tewbre>
        {
            public override int Path => TOP;
            public override int Tier => 5;
            public override int Cost => 123456;
            public override string Icon => "sabre5";
            public override string DisplayName => "I NEED MONEY FOR outrageously incredibly, valuable bananas.";

            public override string Description => "Donate to sabre to get rich";

            public override void ApplyUpgrade(TowerModel towerModel)
            {
                towerModel.GetAttackModel(1).weapons[0].projectile.GetBehavior<CashModel>().maximum = 2000;
                towerModel.GetAttackModel(1).weapons[0].projectile.GetBehavior<CashModel>().minimum = 2000;
                towerModel.GetAttackModel(1).weapons[0].GetBehavior<EmissionsPerRoundFilterModel>().count += 20;
            }
        }
        public class explodeDart : ModUpgrade<tewbre>
        {
            public override int Path => MIDDLE;
            public override int Tier => 1;
            public override int Cost => 400;
            public override string Icon => "tewity";
            public override string DisplayName => "I NEED MONEY FOR THIS UPGRADE SABRE";

            public override string Description => "Tewity purchases an simple upgrade that makes homing darts explode with more homing darts.";

            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var proj = towerModel.GetAttackModel().weapons[0].projectile.Duplicate();
                towerModel.GetAttackModel().weapons[0].projectile.pierce /= 3;
                towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(new CreateProjectileOnContactModel("Projectile_Create",proj, new ArcEmissionModel("ArcEmissionModel_", 4, 0.0f, 360.0f, null, false), true, false, false));
            }
        }
        public class betterHoming : ModUpgrade<tewbre>
        {
            public override int Path => MIDDLE;
            public override int Tier => 2;
            public override int Cost => 850;
            public override string Icon => "tewity2";
            public override string DisplayName => "SABRE GIVE ME MONEEYYYY FOR LEADS";

            public override string Description => "Tewity purcahses a upgrade that allows homing darts to pop lead.";

            public override void ApplyUpgrade(TowerModel towerModel)
            {
                towerModel.GetAttackModel().weapons[0].projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;
            }
        }
        public class moabElimantion : ModUpgrade<tewbre>
        {
            public override int Path => MIDDLE;
            public override int Tier => 3;
            public override int Cost => 2000;
            public override string Icon => "tewity3";
            public override string DisplayName => "SABRE GIVE ME MONEEYYYY FOR MOABS";

            public override string Description => "Tewity purchases a upgrade that gives more darts for homing, and more damage to MOABS.";

            public override void ApplyUpgrade(TowerModel towerModel)
            {
                towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().emission = new ArcEmissionModel("ArcEmissionModel_", 8, 0.0f, 360.0f, null, false);
                towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(new DamageModifierForTagModel("moabmore","Moabs",2f,12f,false, true));
            }
        }
        public class tewbreTime : ModUpgrade<tewbre>
        {
            public override int Path => MIDDLE;
            public override int Tier => 4;
            public override int Cost => 8500;
            public override string Icon => "tewity4";
            public override string DisplayName => "SABRE I NEED THIS ABILITY! MONEY PLS";

            public override string Description => "Tewity purchases a upgrade that gives the ability to activate tewbre time.";

            public override void ApplyUpgrade(TowerModel towerModel)
            {
                towerModel.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
                var abilityModel = Game.instance.model.GetTowerFromId("BoomerangMonkey-040").GetAbility().Duplicate();
                abilityModel.GetBehavior<TurboModel>().extraDamage += 24;
                abilityModel.GetBehavior<TurboModel>().multiplier = 0.5f;
                abilityModel.cooldown *= 0.6f;
                abilityModel.icon =  towerModel.icon;
                towerModel.AddBehavior(abilityModel);
            }
        }
        public class tewbreTimeBOOM : ModUpgrade<tewbre>
        {
            public override int Path => MIDDLE;
            public override int Tier => 5;
            public override int Cost => 69000;
            public override string Icon => "tewity5";
            public override string DisplayName => "SABRE THIS FINAL UPGRADE SHREDS!! MONEY!";

            public override string Description => "Tewity purchases an upgrade that enhances the ability's destruction.";

            public override void ApplyUpgrade(TowerModel towerModel)
            {
                towerModel.GetAbility().GetBehavior<TurboModel>().multiplier *= 0.5f;
                towerModel.GetAbility().GetBehavior<TurboModel>().extraDamage += 50;
                towerModel.GetAbility().cooldown *= 0.6f;
            }
        }
        public class HOLYMACORONI : ModUpgrade<tewbre>
        {
            public override int Path => BOTTOM;
            public override int Tier => 1;
            public override int Cost => 1500;
            public override string Icon => "macoroni";
            public override string DisplayName => "HOLY MACORONI";

            public override string Description => "Throws cheesy macoroni that pops bloons.";

            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var attackModel = towerModel.GetAttackModel();
                var macoroniModel = Game.instance.model.GetTowerFromId("BoomerangMonkey-002").GetAttackModel().weapons[0].Duplicate();
                macoroniModel.projectile.GetDamageModel().damage *= 2;
                macoroniModel.projectile.scale *= 2;
                macoroniModel.rate = 1f;
                macoroniModel.projectile.ApplyDisplay<macoroniDisplay>();
                macoroniModel.projectile.AddBehavior(Game.instance.model.GetTowerFromId("GlueGunner-003").GetAttackModel().weapons[0].projectile.GetBehavior<SlowModel>().Duplicate());
                macoroniModel.projectile.AddBehavior(Game.instance.model.GetTowerFromId("GlueGunner-100").GetAttackModel().weapons[0].projectile.GetBehavior<CreateSoundOnProjectileCollisionModel>().Duplicate());
                macoroniModel.projectile.AddBehavior(Game.instance.model.GetTowerFromId("GlueGunner-100").GetAttackModel().weapons[0].projectile.GetBehavior<SlowModifierForTagModel>().Duplicate());
                attackModel.AddWeapon(macoroniModel);
            }
        }
        public class macoroniDisplay : ModDisplay
        {
            public override string BaseDisplay => Generic2dDisplay;
            public override void ModifyDisplayNode(UnityDisplayNode node)
            {
                Set2DTexture(node, "MacProj");
            }
        }
        public class doofenshmirtzinvention : ModUpgrade<tewbre>
        {
            public override int Path => BOTTOM;
            public override int Tier => 2;
            public override int Cost => 2000;
            public override string Icon => "doofenshmirtz";
            public override string DisplayName => "The doofenshmirtz invention";

            public override string Description => "the secret invention to do what exactly? Tewbre will find out...";

            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var attackModel = towerModel.GetAttackModel();
                var doofenshmirtzModel = Game.instance.model.GetTowerFromId("DartMonkey-005").GetAttackModel().weapons[0].Duplicate();
                doofenshmirtzModel.rate = 2f; 
                doofenshmirtzModel.projectile.pierce *= 4f;
                attackModel.AddWeapon(doofenshmirtzModel);
            }
        }
        public class Ballofdoom : ModUpgrade<tewbre>
        {
            public override int Path => BOTTOM;
            public override int Tier => 3;
            public override int Cost => 5000;
            public override string Icon => "bigDart";
            public override string DisplayName => "The Dart of Doomsville";

            public override string Description => "Some say the dart is very B.A.D";

            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var attackModel = towerModel.GetAttackModel();
                towerModel.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
                var proj = attackModel.weapons[0].Duplicate();
                proj.rate = 15f;
                proj.name = "BIGDART";
                proj.projectile.GetBehavior<TravelStraitModel>().Speed *= 0.1f;
                proj.projectile.GetBehavior<TravelStraitModel>().lifespan *= 6f;
                proj.projectile.GetBehavior<AdoraTrackTargetModel>().maximumSpeed *= 0.1f;
                proj.projectile.GetBehavior<AdoraTrackTargetModel>().minimumSpeed *= 0.1f;
                proj.projectile.GetBehavior<AdoraTrackTargetModel>().lifespan *= 5f;
                proj.projectile.pierce = 1f;
                proj.projectile.scale *= 3;
                proj.projectile.GetDamageModel().damage = 10000f;
                attackModel.AddWeapon(proj);
            }
        }
        public class OhNospaghettios : ModUpgrade<tewbre>
        {
            public override int Path => BOTTOM;
            public override int Tier => 4;
            public override int Cost => 25000;
            public override string Icon => "spagethhi";
            public override string DisplayName => "Oh no spaghettios!";

            public override string Description => "Shoots alot of spaghetti to deal quite a bit of damage and slither bloon to bloon";

            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var attackModel = towerModel.GetAttackModel();
                var proj = Game.instance.model.GetTowerFromId("BoomerangMonkey-400").GetAttackModel().weapons[0].Duplicate();
                proj.emission = new ArcEmissionModel("ArcEmissionModel_", 20, 0.0f, 50.0f, null, false);
                proj.rate = 1f;
                proj.projectile.ApplyDisplay<spaghett>();
                proj.projectile.pierce *= 2;
                proj.projectile.GetDamageModel().damage += 4;
                attackModel.AddWeapon(proj);
            }
        }
        public class spaghett : ModDisplay
        {
            public override string BaseDisplay => Generic2dDisplay;
            public override void ModifyDisplayNode(UnityDisplayNode node)
            {
                Set2DTexture(node, "spaghettiImg");
            }
        }
        public class WaluigiTime : ModUpgrade<tewbre>
        {
            public override int Path => BOTTOM;
            public override int Tier => 5;
            public override int Cost => 100000;
            public override string Icon => "waluigiIcon";
            public override string DisplayName => "Waluigi Time!!!";

            public override string Description => "Wah? Tewbre? WAHHH!!! Waluigi loves bombs! He will gladly give you nukes! Hooray!";

            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var attackModel = towerModel.GetAttackModel();
                var newproj = Game.instance.model.GetTowerFromId("BombShooter-500").GetAttackModel().weapons[0].Duplicate();
                newproj.rate = 1f;
                attackModel.AddWeapon(newproj);
                foreach (var weaponModel in towerModel.GetWeapons())
                {
                    weaponModel.rate *= 0.5f;
                }
                foreach (var weaponModel in towerModel.GetWeapons())
                {
                    if (weaponModel.name.Contains("BIGDART"))
                    {
                        weaponModel.projectile.GetDamageModel().damage *= 6;
                        weaponModel.projectile.scale *= 1.5f;
                    }

                }
            }
        }
        public class TewbreDaBomb : ModParagonUpgrade<tewbre>
        {
            public override int Cost => 800000;
            public override string Description => "Combine every thing and multiply it's power by 1000 - Tewbre 2023";
            public override string DisplayName => "Tewbre Is da BOMB";
            public override string Portrait => "Icon";
            public override string Icon => "Icon";
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                towerModel.range += 50;
                var attackModel = towerModel.GetAttackModel();
                attackModel.range += 50;
                towerModel.GetAttackModel(1).weapons[0].projectile.GetBehavior<CashModel>().maximum = 2000;
                towerModel.GetAttackModel(1).weapons[0].projectile.GetBehavior<CashModel>().minimum = 2000;
                towerModel.GetAttackModel(1).weapons[0].GetBehavior<EmissionsPerRoundFilterModel>().count = 50;
                var proj = attackModel.weapons[0].Duplicate();
                proj.rate = 10f;
                proj.name = "BIGDART";
                proj.projectile.GetBehavior<TravelStraitModel>().Speed *= 0.1f;
                proj.projectile.GetBehavior<TravelStraitModel>().lifespan *= 6f;
                proj.projectile.GetBehavior<AdoraTrackTargetModel>().maximumSpeed *= 0.1f;
                proj.projectile.GetBehavior<AdoraTrackTargetModel>().minimumSpeed *= 0.1f;
                proj.projectile.GetBehavior<AdoraTrackTargetModel>().lifespan *= 5f;
                proj.projectile.pierce = 20f;
                proj.projectile.scale *= 8;
                proj.projectile.GetDamageModel().damage = 500000f;
                attackModel.AddWeapon(proj);
                var abilityModel = Game.instance.model.GetTowerFromId("BoomerangMonkey-040").GetAbility().Duplicate();
                abilityModel.GetBehavior<TurboModel>().extraDamage += 50;
                abilityModel.GetBehavior<TurboModel>().multiplier = 0.5f;
                abilityModel.cooldown *= 0.3f;
                abilityModel.icon = towerModel.icon;
                towerModel.AddBehavior(abilityModel);
                var thisPorj = towerModel.GetAttackModel().weapons[0].projectile.Duplicate();
                towerModel.GetAttackModel().weapons[0].rate *= 0.2f;
                towerModel.GetAttackModel().weapons[0].projectile.GetDamageModel().damage += 100;
                towerModel.GetAttackModel().weapons[0].projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;
                towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(new CreateProjectileOnContactModel("Projectile_Create2", thisPorj, new ArcEmissionModel("ArcEmissionModel_", 32, 0.0f, 360.0f, null, false), true, false, false));
                var proj2 = Game.instance.model.GetTowerFromId("BoomerangMonkey-402").GetAttackModel().weapons[0].Duplicate();
                proj2.emission = new ArcEmissionModel("ArcEmissionModel_", 32, 0.0f, 360.0f, null, false);
                proj2.rate = 0.3f;
                proj2.projectile.ApplyDisplay<spaghett>();
                proj2.projectile.pierce *= 2;
                proj2.projectile.GetDamageModel().damage += 25;
                attackModel.AddWeapon(proj2);
                towerModel.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
            }
        }
    }
}