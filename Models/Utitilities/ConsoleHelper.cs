using System;
using System.Collections.Generic;

namespace ProjectSolamnia
{
    public static class ConsoleHelpers
    {
        private static CharacterService _characterService = null!;
        public static string? ReadOptionalString(string prompt)
        {
            Console.Write(prompt);
            var s = Console.ReadLine();
            return string.IsNullOrWhiteSpace(s) ? null : s;
        }

        public static int? ReadOptionalInt(string prompt)
        {
            Console.Write(prompt);
            var s = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(s)) return null;
            return int.TryParse(s, out var val) ? val : (int?)null;
        }

        public static TEnum? ReadOptionalEnum<TEnum>(string prompt) where TEnum : struct
        {
            Console.Write(prompt);
            var s = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(s)) return null;
            return Enum.TryParse<TEnum>(s, out var val) ? val : (TEnum?)null;
        }

        public static List<int> ParseIdList(string? input)
        {
            return input?.Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(x => int.TryParse(x.Trim(), out var id) ? id : -1)
                .Where(x => x != -1).ToList() ?? new List<int>();
        }

        // Basit evet/hayır onayı
        public static bool Confirm(string prompt = "Are you sure? [y/N]: ")
        {
            Console.Write(prompt);
            var s = (Console.ReadLine() ?? "").Trim().ToLowerInvariant();
            return s is "y" or "yes";
        }

        public static void ListCharactersBrief()
        {
            var list = _characterService.GetAllCharacters(); // yoksa servise ekleyelim
            if (list == null || !list.Any())
            {
                Console.WriteLine("No characters in DB.");
                return;
            }

            Console.WriteLine("\n-- Characters --");
            foreach (var c in list)
                Console.WriteLine($"ID={c.Id}  Name={c.Name}  Level={c.Level}  Status={c.Status}");
        }

    }
}
