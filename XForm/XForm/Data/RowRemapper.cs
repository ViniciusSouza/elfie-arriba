﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

using XForm.Data;

namespace XForm.Transforms
{
    /// <summary>
    ///  RowRemapper is used by row filtering operations.
    ///  The filter computes the rows to include as indices from 0 to Count - 1.
    ///  This class then converts those matching row indices to real indices on the array for each column.
    ///  This is needed because each column from a source can be mapped differently.
    /// </summary>
    public class RowRemapper
    {
        public int[] MatchingRowIndices;
        public int Count;
        private Dictionary<ArraySelector, ArraySelector> _cachedRemappings;

        public RowRemapper()
        {
            _cachedRemappings = new Dictionary<ArraySelector, ArraySelector>();
        }

        public void ClearAndSize(int length)
        {
            // Ensure the row index array is large enough
            Allocator.AllocateToSize(ref MatchingRowIndices, length);

            // Mark the array empty
            Count = 0;

            // Clear cached remappings (they will need to be recomputed)
            _cachedRemappings.Clear();
        }

        public void Add(int index)
        {
            MatchingRowIndices[Count++] = index;
        }

        public DataBatch Remap(DataBatch source, ref int[] remapArray)
        {
            // See if we have the remapping cached already
            ArraySelector cachedMapping;
            if (_cachedRemappings.TryGetValue(source.Selector, out cachedMapping)) return source.Reselect(cachedMapping);

            DataBatch remapped = source.Select(ArraySelector.Map(MatchingRowIndices, Count), ref remapArray);

            // Cache the remapping
            _cachedRemappings[source.Selector] = remapped.Selector;

            return remapped;
        }
    }
}