To regenerate this table, uncomment the `GenerateCommandMd` function in `Plugin.ValidateCommandPermissions`. Then check the LogOutput.log in the server after starting.
Usage arguments: <> are required, [] are optional

| Command                     | Short hand     | Usage                                                    | Description                                                                                                  | Admin | Level |
|-----------------------------|----------------|----------------------------------------------------------|--------------------------------------------------------------------------------------------------------------|-------|-------|
| `.autorespawn`              | `.autor`       | `[playerName]`                                           | Toggle auto respawn on the same position on death for yourself or a player.                                  |   ☑   | `100` |
| `.autorespawn-all`          | `.autor-all`   |                                                          | Toggle auto respawn on the same position on death for all players.                                           |   ☑   | `100` |
| `.ban info`                 | `.ban i`       | `<playername>`                                           | Check the status of specified player                                                                         |   ☐   | `0`   |
| `.ban player`               | `.ban p`       | `<playername> <days> "<reason>"`                         | Ban a player, 0 days is permanent.                                                                           |   ☐   | `100` |
| `.ban unban`                | `.ban u`       | `<playername>`                                           | Unban the specified player.                                                                                  |   ☐   | `100` |
| `.bloodline add`            | `.bl a`        | `<BloodlineName> <amount>`                               | Adds amount to the specified bloodline. able to use default names, bloodtype names, or the configured names. |   ☑   | `100` |
| `.bloodline get`            | `.bl g`        |                                                          | Display your current bloodline progression                                                                   |   ☐   | `0`   |
| `.bloodline get-all`        | `.bl ga`       |                                                          | Display all your bloodline progressions                                                                      |   ☐   | `0`   |
| `.bloodline log`            | `.bl l`        |                                                          | Toggles logging of bloodlineXP gain.                                                                         |   ☐   | `0`   |
| `.bloodline reset`          | `.bl r`        | `<bloodline>`                                            | Resets a bloodline to gain more power with it.                                                               |   ☐   | `0`   |
| `.bloodline set`            | `.bl s`        | `<playerName> <bloodline> <value>`                       | Sets the specified players bloodline to a specific value                                                     |   ☑   | `100` |
| `.experience ability`       | `.xp a`        | `<AbilityName> <amount>`                                 | Spend given points on given ability                                                                          |   ☐   | `0`   |
| `.experience ability reset` | `.xp ar`       |                                                          | Reset your spent ability points                                                                              |   ☐   | `50`  |
| `.experience ability show`  | `.xp as`       |                                                          | Display the buffs provided by the XP class system                                                            |   ☐   | `0`   |
| `.experience get`           | `.xp g`        |                                                          | Display your current xp                                                                                      |   ☐   | `0`   |
| `.experience log`           | `.xp l`        |                                                          | Toggles logging of xp gain.                                                                                  |   ☐   | `0`   |
| `.experience set`           | `.xp s`        | `<playerName> <XP>`                                      | Sets the specified player's current xp to the given value                                                    |   ☑   | `100` |
| `.godmode`                  | `.gm`          |                                                          | Toggles god mode for the current user                                                                        |   ☐   | `100` |
| `.kick`                     |                | `<playername>`                                           | Kick the specified player out of the server.                                                                 |   ☑   | `100` |
| `.kit`                      |                | `<Name>`                                                 | Gives you a previously specified set of items.                                                               |   ☐   | `100` |
| `.mastery add`              | `.m a`         | `<weaponType> <amount>`                                  | Adds the amount to the mastery of the specified weaponType                                                   |   ☑   | `100` |
| `.mastery get`              | `.m g`         | `[weaponType]`                                           | Display your current mastery progression for your equipped or specified weapon type                          |   ☐   | `0`   |
| `.mastery get-all`          | `.m ga`        |                                                          | Display your current mastery progression in everything                                                       |   ☐   | `0`   |
| `.mastery log`              | `.m l`         |                                                          | Toggles logging of mastery gain.                                                                             |   ☐   | `0`   |
| `.mastery reset`            | `.m r`         | `<weaponType>`                                           | Resets a mastery to gain more power with it.                                                                 |   ☐   | `0`   |
| `.mastery set`              | `.m s`         | `<playerName> <weaponType> <masteryValue>`               | Sets the specified player's mastery to a specific value                                                      |   ☑   | `100` |
| `.nocooldown`               | `.nocd`        |                                                          | Toggles instant cooldown for all abilities.                                                                  |   ☑   | `100` |
| `.playerinfo`               | `.pi`          | `[PlayerName]`                                           | Display the player information details.                                                                      |   ☐   | `0`   |
| `.powerdown`                | `.pd`          | `<playerName>`                                           | Remove power up buff from the player.                                                                        |   ☑   | `100` |
| `.powerup`                  | `.pu`          | `<player_name> <max hp> <p.atk> <s.atk> <p.def> <s.def>` | Buff player with the given values.                                                                           |   ☑   | `100` |
| `.re disable`               |                |                                                          | Disables the random encounter timer.                                                                         |   ☑   | `100` |
| `.re enable`                |                |                                                          | Enables the random encounter timer.                                                                          |   ☑   | `100` |
| `.re me`                    |                |                                                          | Starts an encounter for the admin who sends the command.                                                     |   ☑   | `100` |
| `.re player`                |                | `<PlayerName>`                                           | Starts an encounter for the given player.                                                                    |   ☑   | `100` |
| `.re start`                 |                |                                                          | Starts an encounter for a random online user.                                                                |   ☑   | `100` |
| `.save`                     |                |                                                          | Force the server to write OpenRPG DB to file.                                                                |   ☑   | `100` |
| `.speed`                    |                |                                                          | Toggles increased movement speed.                                                                            |   ☑   | `100` |
| `.sunimmunity`              | `.si`          |                                                          | Toggles sun immunity.                                                                                        |   ☐   | `100` |
| `.unlock achievements`      | `.u a`         | `[PlayerName]`                                           | Unlock all shapeshifters that drop from killing a VBood for yourself or a player.                            |   ☑   | `100` |
| `.unlock research`          | `.u r`         | `[PlayerName]`                                           | Unlock all shapeshifters that drop from killing a VBood for yourself or a player.                            |   ☑   | `100` |
| `.unlock vbloodability`     | `.u vba`       | `[PlayerName]`                                           | Unlock all abilities that drop from killing a VBood for yourself or a player.                                |   ☑   | `100` |
| `.unlock vbloodpassive`     | `.u vbp`       | `[PlayerName]`                                           | Unlock all passives that drop from killing a VBood for yourself or a player.                                 |   ☑   | `100` |
| `.unlock vbloodshapeshift`  | `.u vbs`       | `[PlayerName]`                                           | Unlock all shapeshifters that drop from killing a VBood for yourself or a player.                            |   ☑   | `100` |
| `.wanted fixminions`        | `.w fm`        |                                                          | Remove broken gloomrot technician units                                                                      |   ☑   | `100` |
| `.wanted get`               | `.w g`         |                                                          | Shows your current wanted level                                                                              |   ☐   | `0`   |
| `.wanted log`               | `.w l`         |                                                          | Toggle logging of heat data.                                                                                 |   ☐   | `0`   |
| `.wanted set`               | `.w s`         | `<name> <faction> <value>`                               | Sets the current wanted level                                                                                |   ☑   | `100` |
| `.wanted trigger`           | `.w t`         | `<name>`                                                 | Triggers the ambush check for the given user                                                                 |   ☑   | `100` |
| `.waypoint go`              | `.wp g`        | `<waypoint name>`                                        | Teleports you to the specified waypoint                                                                      |   ☐   | `100` |
| `.waypoint list`            | `.wp l`        |                                                          | lists waypoints available to you                                                                             |   ☐   | `0`   |
| `.waypoint remove`          | `.wp r`        | `<waypoint name>`                                        | Removes the specified personal waypoint                                                                      |   ☐   | `100` |
| `.waypoint remove global`   | `.wp rg`       | `<waypoint name>`                                        | Removes the specified global waypoint                                                                        |   ☑   | `100` |
| `.waypoint set`             | `.wp s`        | `<waypoint name>`                                        | Creates the specified personal waypoint                                                                      |   ☑   | `100` |
| `.waypoint set global`      | `.wp sg`       | `<waypoint name>`                                        | Creates the specified global waypoint                                                                        |   ☑   | `100` |
| `.worlddynamics ignore`     | `.wd ignore`   | `<npc prefab name>`                                      | Ignores a specified mob for buffing.                                                                         |   ☑   | `100` |
| `.worlddynamics info`       | `.wd info`     | `[faction]`                                              | List faction stats of all active factions or given faction                                                   |   ☐   | `0`   |
| `.worlddynamics load`       | `.wd load`     |                                                          | Load from the json file.                                                                                     |   ☑   | `100` |
| `.worlddynamics save`       | `.wd save`     |                                                          | Save to the json file.                                                                                       |   ☑   | `100` |
| `.worlddynamics unignore`   | `.wd unignore` | `<npc prefab name>`                                      | Removes a mob from the world dynamics ignore list.                                                           |   ☑   | `100` |