﻿using System.Collections.Generic;

namespace Imagin.Common.Input.HotKeys
{
    /// <summary>
    /// A collection of HotKeySets
    /// </summary>
    public sealed class HotKeySetCollection : List<HotKeySet>
    {
        delegate void KeyChainHandler( KeyEventArgsExt kex );

        KeyChainHandler m_keyChain;

        ///<summary>
        /// Adds a HotKeySet to the collection.
        ///</summary>
        ///<param name="hks"></param>
        public new void Add(HotKeySet hks)
        {
            m_keyChain += hks.OnKey;
            base.Add( hks );
        }

        ///<summary>
        /// Removes the HotKeySet from the collection.
        ///</summary>
        ///<param name="hks"></param>
        public new void Remove(HotKeySet hks)
        {
            m_keyChain -= hks.OnKey;
            base.Remove( hks );
        }

        /// <summary>
        /// Uses a multi-case delegate to invoke individual HotKeySets if the Key is in use by any HotKeySets.
        /// </summary>
        /// <param name="e"></param>
        internal void OnKey(KeyEventArgsExt e)
        {
            if ( m_keyChain != null )
                m_keyChain( e );
        }
    }
}
