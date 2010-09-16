// Copyright 2005-2006 University of Wisconsin
// All rights reserved. 
//
// The copyright holders license this file under the New (3-clause) BSD
// License (the "License").  You may not use this file except in
// compliance with the License.  A copy of the License is available at
//
//   http://www.opensource.org/licenses/bsd-license.php
//
// and is included in the NOTICE.txt file distributed with this work.
//
// Contributors:
//   James Domingo, UW-Madison, Forest Landscape Ecology Lab

namespace Landis.SpatialModeling.Implementation
{
    /// <summary>
    /// An output raster file to which pixel data are written.
    /// </summary>
    /// <remarks>
    /// A helper class for implementing the IOutputRaster interface.
    /// </remarks>
    public abstract class OutputRaster
        : Raster
    {
        private int pixelsWritten;
        private bool disposed;

        //---------------------------------------------------------------------

        /// <summary>
        /// The number of pixels written to the raster.
        /// </summary>
        public int PixelsWritten
        {
            get {
                if (disposed)
                    throw new System.ObjectDisposedException(null);
                return pixelsWritten;
            }
        }

        //---------------------------------------------------------------------

        public OutputRaster(string     path,
                            Dimensions dimensions)
            : base(path, dimensions)
        {
            this.pixelsWritten = 0;
            this.disposed = false;
        }

        //---------------------------------------------------------------------

        /// <summary>
        /// Increments the number of pixels written by 1.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">
        /// The number of pixels written is equal to the number of pixels
        /// allowed by the raster's dimensions.
        /// </exception>
        /// <remarks>
        /// The WritePixel method of derived classes should call this method
        /// before actually writing a pixel's data.
        /// </remarks>
        protected void IncrementPixelsWritten()
        {
            if (disposed)
                throw new System.ObjectDisposedException(null);
            if (pixelsWritten >= PixelCount)
                throw new System.InvalidOperationException("Trying to write extra pixel to output raster");
            pixelsWritten++;
        }

        //---------------------------------------------------------------------

        /// <summary>Dispose unmanaged resources and possibly managed
        /// resources as well.
        /// </summary>
        /// <param name="disposeManaged">
        /// If true, then managed resources should be disposed along with
        /// unmanaged resources.  If false, then only unmanaged resources
        /// should be disposed.
        /// </param>
        protected override void Dispose(bool disposeManaged)
        {
            if (! disposed) {
                if (pixelsWritten < PixelCount) {
                    PartialOutputRaster.Closed(this);
                }
            }
            disposed = true;
            base.Dispose(disposeManaged);
        }
    }
}
