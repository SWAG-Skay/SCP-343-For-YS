using Exiled.API.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCP_343_For_YS
{
    public class Translations : ITranslation
    {
        /// <summary>
        /// Dictionary containing localized messages for supported languages.
        /// </summary>
        private readonly Dictionary<string, Dictionary<string, string>> _messages = new Dictionary<string, Dictionary<string, string>>
        {
            {
                "en", new Dictionary<string, string>
                {
                    { "SpawnMessge", "You are SCP-343" },
                    { "TelepeortMessge", "<color=#00FF00>You have been teleported</color>" },
                    { "NoPlayersForTp", "<color=red>There are no suitable players for teleportation!</color>" },
                    { "CDMessage", "<color=yellow>Cooldown {0:0} sec.</color>" },
                    { "InteractingWith330", "<color=red>Undo action! Made to remove bugs</color>" },
                    { "SCP343LinksPlayers", "<color=red>You can't link players!</color>" },
                    { "PlayersBind343", "<color=red>You cannot bind SCP-343</color>" },
                    { "Spawn343CommandDescription", "Spawns a player as SCP-343" },
                    { "FlyCommandDescription", "Activates flight mode for SCP-343 for 25 seconds" },
                    { "CommandOnlyForConsole", "This command is only available through the admin console!" },
                    { "CommandOnlyForPlayers", "This command is only available to players!" },
                    { "InsufficientPermissions", "Insufficient permissions to use this command!" },
                    { "InvalidPlayerId", "Invalid player ID or player not found!" },
                    { "PlayerMustBeAlive", "The target player must be alive!" },
                    { "AlreadySCP343", "{0} is already SCP-343!" },
                    { "SpawnSuccess", "{0} is now SCP-343!" },
                    { "PlayerNotFound", "Could not find player!" },
                    { "OnlySCP343", "Only SCP-343 can use this command!" },
                    { "FlyCooldown", "Flight mode is on cooldown for {0:0} seconds!" },
                    { "FlyCooldownHint", "Wait {0:0} seconds before using flight mode again!" },
                    { "FlyActivated", "Flight mode activated!" },
                    { "FlyEnded", "Flight mode has ended!" }
                }
            },
            {
                "ru", new Dictionary<string, string>
                {
                    { "SpawnMessge", "Вы стали SCP-343" },
                    { "TelepeortMessge", "<color=#00FF00>Вы телепортированы</color>" },
                    { "NoPlayersForTp", "<color=red>Нет подходящих игроков для телепортации!</color>" },
                    { "CDMessage", "<color=yellow>Перезарядка {0:0} сек.</color>" },
                    { "InteractingWith330", "<color=red>Действие отменено! Сделано для устранения ошибок</color>" },
                    { "SCP343LinksPlayers", "<color=red>Вы не можете связывать игроков!</color>" },
                    { "PlayersBind343", "<color=red>Вы не можете связать SCP-343</color>" },
                    { "Spawn343CommandDescription", "Создаёт игрока как SCP-343" },
                    { "FlyCommandDescription", "Активирует режим полёта для SCP-343 на 25 секунд" },
                    { "CommandOnlyForConsole", "Эта команда доступна только через админ-консоль!" },
                    { "CommandOnlyForPlayers", "Эта команда доступна только для игроков!" },
                    { "InsufficientPermissions", "Недостаточно прав для использования этой команды!" },
                    { "InvalidPlayerId", "Неверный ID игрока или игрок не найден!" },
                    { "PlayerMustBeAlive", "Целевой игрок должен быть жив!" },
                    { "AlreadySCP343", "{0} уже является SCP-343!" },
                    { "SpawnSuccess", "{0} теперь SCP-343!" },
                    { "PlayerNotFound", "Игрок не найден!" },
                    { "OnlySCP343", "Только SCP-343 может использовать эту команду!" },
                    { "FlyCooldown", "Режим полёта на перезарядке {0:0} секунд!" },
                    { "FlyCooldownHint", "Подождите {0:0} секунд перед повторным использованием режима полёта!" },
                    { "FlyActivated", "Режим полёта активирован!" },
                    { "FlyEnded", "Режим полёта завершён!" }
                }
            },
            {
                "ja", new Dictionary<string, string>
                {
                    { "SpawnMessge", "あなたはSCP-343になりました" },
                    { "TelepeortMessge", "<color=#00FF00>テレポートしました</color>" },
                    { "NoPlayersForTp", "<color=red>テレポートに適したプレイヤーがいません！</color>" },
                    { "CDMessage", "<color=yellow>クールダウン {0:0} 秒</color>" },
                    { "InteractingWith330", "<color=red>アクションが取り消されました！バグ修正のため</color>" },
                    { "SCP343LinksPlayers", "<color=red>プレイヤーを拘束できません！</color>" },
                    { "PlayersBind343", "<color=red>SCP-343を拘束できません</color>" },
                    { "Spawn343CommandDescription", "プレイヤーをSCP-343としてスポーンします" },
                    { "FlyCommandDescription", "SCP-343のフライモードを25秒間有効にします" },
                    { "CommandOnlyForConsole", "このコマンドは管理コンソールからのみ使用可能です！" },
                    { "CommandOnlyForPlayers", "このコマンドはプレイヤーのみ使用可能です！" },
                    { "InsufficientPermissions", "このコマンドを使用する権限が不足しています！" },
                    { "InvalidPlayerId", "無効なプレイヤーIDまたはプレイヤーが見つかりません！" },
                    { "PlayerMustBeAlive", "対象プレイヤーは生きている必要があります！" },
                    { "AlreadySCP343", "{0}はすでにSCP-343です！" },
                    { "SpawnSuccess", "{0}がSCP-343になりました！" },
                    { "PlayerNotFound", "プレイヤーが見つかりません！" },
                    { "OnlySCP343", "このコマンドはSCP-343のみ使用可能です！" },
                    { "FlyCooldown", "フライモードは{0:0}秒間クールダウン中です！" },
                    { "FlyCooldownHint", "フライモードを再度使用するには{0:0}秒待ってください！" },
                    { "FlyActivated", "フライモードが有効になりました！" },
                    { "FlyEnded", "フライモードが終了しました！" }
                }
            },
            {
                "zh", new Dictionary<string, string>
                {
                    { "SpawnMessge", "你已成为SCP-343" },
                    { "TelepeortMessge", "<color=#00FF00>你已传送</color>" },
                    { "NoPlayersForTp", "<color=red>没有适合传送的玩家！</color>" },
                    { "CDMessage", "<color=yellow>冷却时间 {0:0} 秒</color>" },
                    { "InteractingWith330", "<color=red>操作取消！为修复错误而设计</color>" },
                    { "SCP343LinksPlayers", "<color=red>你不能束缚其他玩家！</color>" },
                    { "PlayersBind343", "<color=red>你不能束缚SCP-343</color>" },
                    { "Spawn343CommandDescription", "将玩家生成为主SCP-343" },
                    { "FlyCommandDescription", "为SCP-343激活25秒的飞行模式" },
                    { "CommandOnlyForConsole", "此命令仅可通过管理控制台使用！" },
                    { "CommandOnlyForPlayers", "此命令仅限玩家使用！" },
                    { "InsufficientPermissions", "权限不足，无法使用此命令！" },
                    { "InvalidPlayerId", "无效的玩家ID或玩家未找到！" },
                    { "PlayerMustBeAlive", "目标玩家必须存活！" },
                    { "AlreadySCP343", "{0}已经是SCP-343！" },
                    { "SpawnSuccess", "{0}现在是SCP-343！" },
                    { "PlayerNotFound", "未找到玩家！" },
                    { "OnlySCP343", "此命令仅限SCP-343使用！" },
                    { "FlyCooldown", "飞行模式冷却中，剩余{0:0}秒！" },
                    { "FlyCooldownHint", "请等待{0:0}秒再次使用飞行模式！" },
                    { "FlyActivated", "飞行模式已激活！" },
                    { "FlyEnded", "飞行模式已结束！" }
                }
            }
        };

        /// <summary>
        /// Retrieves a localized message based on the specified key and language.
        /// </summary>
        /// <param name="key">The message key.</param>
        /// <param name="language">The language code (en, ru, ja, zh, or custom).</param>
        /// <returns>The localized message or the config value if language is 'custom'.</returns>
        public string GetMessage(string key, string language)
        {
            if (language.ToLower() == "custom")
            {
                return key switch
                {
                    "SpawnMessge" => Plugin.Instance.Config.SpawnMessage,
                    "TelepeortMessge" => Plugin.Instance.Config.TeleportMessage,
                    "NoPlayersForTp" => Plugin.Instance.Config.NoPlayersForTp,
                    "CDMessage" => Plugin.Instance.Config.CDMessage,
                    "InteractingWith330" => Plugin.Instance.Config.InteractingWith330,
                    "SCP343LinksPlayers" => Plugin.Instance.Config.SCP343LinksPlayers,
                    "PlayersBind343" => Plugin.Instance.Config.PlayersBind343,
                    _ => _messages["en"][key] // Fallback to English
                };
            }

            return _messages.TryGetValue(language.ToLower(), out var messages) && messages.TryGetValue(key, out var message)
                ? message
                : _messages["en"][key]; // Fallback to English
        }
    }
}