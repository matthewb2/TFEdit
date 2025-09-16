"""
use strict;
use warnings;
use Cwd;

use File::Find;
"""
from ast import Break
import sys
import os
import re
import glob

#print(glob.glob(glob.escape(os.getcwd()) + "/*.txt"))


if len(sys.argv) < 3:
  print ("Useage: python {0} [string to search] [file type]\n".format(sys.argv[0]))
  sys.exit()

search_pattern = sys.argv[1]
#file_pattern  = sys.argv[1]

def dir_match(file):
  try: 
    f = open(file, 'r', encoding="ISO-8859-1")
    Lines = f.readlines()
    #print(file) 
    for line in Lines:
      TEXTO = sys.argv[1]
      my_regex = r"\b(?=\w)" + re.escape(TEXTO) + r"\b(?!\w)"
      if re.search(my_regex, line, re.IGNORECASE):
        print("found {0} in {1}\n".format(TEXTO, file))
        break
    f.close()
  except:
    Break
#search_pattern = "hello"

from pathlib import Path
result = list(Path(os.getcwd()).glob('**/*.'+sys.argv[2]))

for fi in result:
  #print(fi)
  dir_match(fi)
        