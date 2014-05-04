using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Planspelet
{
    class PublishMenu: Tab
    {
        Book activeBook;

        public PublishMenu(Vector2 position, float scale)
            :base(position, scale)
        {
        }

        public void SetActiveBook(Book book)
        {
            activeBook = book;
        }
    }
}
