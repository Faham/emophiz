splitCSV() {
	START_TIME=$1
	END_TIME=$2
	IN_FILE=$3
	OUT_FILE=$4

	gawk -F"," "BEGIN { c=0; } { c++; if (c == 1 || (\$1 >= $START_TIME && \$1 <= $END_TIME)) print \$line; }" $IN_FILE > $OUT_FILE
}

IN_FILE=$9
FILENAME="${IN_FILE%.csv}"
FILENAME="${FILENAME#*/}"

for ((i=1;i<=7;i+=2)); do
	PART=$(($i / 2 + 1))
	OUT_FILE=${FILENAME}_${PART}.csv
	$(splitCSV ${BASH_ARGV[$((9-$i))]} ${BASH_ARGV[$((9-$i-1))]} $IN_FILE `eval echo "splitted/$OUT_FILE"`)
done;
