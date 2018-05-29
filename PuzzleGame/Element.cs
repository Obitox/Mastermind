using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleGame
{
    public class Element
    {
        protected int isTrue;
        protected int isFalse;
        protected int numberOfOccurence;

        public int getIsTrue()
        {
            return isTrue;
        }

        public int getIsFalse()
        {
            return isFalse;
        }

        public int getNumberOfOccurence()
        {
            return numberOfOccurence;
        }

        public void setNumberOfOccurence()
        {
            numberOfOccurence++;
        }
        public void setIsTrue()
        {
            isTrue++;
        }
        public void setIsFalse()
        {
            isFalse++;
        }

        public void resetNumberOfOccurence()
        {
            numberOfOccurence = 0;
        }
        public void resetIsTrue()
        {
            isTrue = 0;
        }
        public void resetIsFalse()
        {
            isFalse = 0;
        }
    }
    
}
