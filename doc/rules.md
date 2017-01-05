# Modding rules

Payload studios has reached out to the TerraTech modding community as a result of its rapid growth. They want to set a number of restrictions to ensure that the modding community won't upset the development of the game.

## Rules for modders
1. Do not leak the game freely to the public
2. Do not reveal secrets or hidden information, such as cheat codes, blocks, corperations, gamemodes and the likes.
3. Make your mod flag output and saves that the game has been modded.
4. Do not break the age-rating set by Payload Studios (they currently aim for children ~8 years old)
5. Do not upset the development of TerraTech in any significant way
6. Do not encourage breaking of these rules to others
7. Do not significantly upset the game balance of survival mode

## Flagging modded games

The development team doesn't want to spend time on bugs originating from mods as they may not actually existin the vanilla version of the game. To this extend, modders are asked to flag the following:

1. `output_log.txt` with the message `This game is modded`
2. In `UIScreenBugReport` modify `WWWForm` in `PostIt();` and add a field `mods` set to any string value.
3. In `UIScreenBugReport` prefix the usermessage inserted into the `WWWForm` `body` field with `This game is modded\n\n`
4. Flag saves by setting `m_OverlayData` to `This game is modded`
