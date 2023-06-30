echo "Auto update project on Git with version $1"
#git init
git add .
git commit -m "Update_$1"
git branch -l
#git checkout -q dev-Jonny
#git remote add origin https://github.com/UIX-Dev/Ilwall-Prototype.git
git push origin master

