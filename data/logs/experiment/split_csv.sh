START_TIME=$1
END_TIME=$2
IN_FILE=$3
OUT_FILE=$4

gawk -F"," "BEGIN { c=0; } { c++; if (c == 1 || (\$1 >= $START_TIME && \$1 <= $END_TIME)) print \$line; }" $IN_FILE > $OUT_FILE