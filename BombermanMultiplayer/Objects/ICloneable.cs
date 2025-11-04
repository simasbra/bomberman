using System;

namespace BombermanMultiplayer
{
    /// <summary>
    /// Prototype ðablono sàsaja - naudojama objektø klonëjimui
    /// </summary>
    public interface ICloneable
    {
        /// <summary>
        /// Sukuria pavirðinæ (shallow) objekto kopija - tik pagrindiniai atributai
        /// </summary>
        /// <returns>Shallow kopija</returns>
        object Clone();

        /// <summary>
        /// Sukuria giliàjà (deep) objekto kopija - visi nested objektai taip pat kopijuojami
        /// </summary>
        /// <returns>Deep kopija</returns>
        object DeepClone();
    }
}
