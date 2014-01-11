DEST=/home/ejjong/RiderStatus/
rm -v ${DEST}*
rsync -av --exclude Status.*  /var/TeamCity/buildAgent/work/f5a388f2ffffb4ec/output/Release/ ${DEST}