﻿/*
      █▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀ ▀▀▀▀▀▀▀▀▀▀▀▀▀▀ ▀▀▀  ▀  ▀      ▀▀
      █
      █   ▄████████
      █   ███    ███
      █   ███    █▀   ██████  ██▄▄▄▄    ▄█████     ██      ▄█████  ██▄▄▄▄      ██      ▄█████
      █   ███        ██    ██ ██▀▀▀█▄   ██  ▀  ▀███████▄   ██   ██ ██▀▀▀█▄ ▀███████▄   ██  ▀
      █   ███        ██    ██ ██   ██   ██         ██  ▀   ██   ██ ██   ██     ██  ▀   ██
      █   ███    █▄  ██    ██ ██   ██ ▀███████     ██    ▀████████ ██   ██     ██    ▀███████
      █   ███    ███ ██    ██ ██   ██    ▄  ██     ██      ██   ██ ██   ██     ██       ▄  ██
      █   ████████▀   ██████   █   █   ▄████▀     ▄██▀     ██   █▀  █   █     ▄██▀    ▄████▀
      █
 ▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄ ▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄ ▄▄  ▄▄ ▄▄   ▄▄▄▄ ▄▄     ▄▄     ▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄ ▄ ▄
 █████████████████████████████████████████████████████████████ ███████████████ ██  ██ ██   ████ ██     ██     ████████████████ █ █
      ▄
      █  Constants for the Package namespace.
      █
      █▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀ ▀▀▀▀▀▀▀▀▀▀▀ ▀ ▀▀▀     ▀▀               ▀
      █  The GNU Affero General Public License (GNU AGPL)
      █
      █  Copyright (C) 2016 JP Dillingham (jp@dillingham.ws)
      █
      █  This program is free software: you can redistribute it and/or modify
      █  it under the terms of the GNU Affero General Public License as published by
      █  the Free Software Foundation, either version 3 of the License, or
      █  (at your option) any later version.
      █
      █  This program is distributed in the hope that it will be useful,
      █  but WITHOUT ANY WARRANTY; without even the implied warranty of
      █  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
      █  GNU Affero General Public License for more details.
      █
      █  You should have received a copy of the GNU Affero General Public License
      █  along with this program.  If not, see <http://www.gnu.org/licenses/>.
      █
      ▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀  ▀▀ ▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀██
                                                                                                   ██
                                                                                               ▀█▄ ██ ▄█▀
                                                                                                 ▀████▀
                                                                                                   ▀▀                            */

namespace OpenIIoT.SDK
{
    /// <summary>
    ///     Constants for the SDK namespace.
    /// </summary>
    public static class Constants
    {
        #region Public Fields

        /// <summary>
        ///     The OpenIIoT PGP Public Key.
        /// </summary>
        public const string PGPPublicKey = "-----BEGIN PGP PUBLIC KEY BLOCK----- Version: Keybase OpenPGP v2.0.64 Comment: https://keybase.io/crypto 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 =51g7 -----END PGP PUBLIC KEY BLOCK-----";

        #endregion Public Fields
    }
}