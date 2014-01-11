DEST=/home/ejjong/RiderStatus/
cd ${DEST}
rm -rf ^Status
rsync -av --exclude Status.*  /var/TeamCity/buildAgent/work/f5a388f2ffffb4ec/output/Release/ ${DEST}