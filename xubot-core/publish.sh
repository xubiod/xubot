# publish xubot to /home/[USER]/xubot-home
# this is to provide a consistent home for xubot
echo "1/3 -> publish xubot to ~/xubot-home"
dotnet publish --configuration Release --output ~/xubot-home

# move to xubot's home
# this instance it's not breaking and entering
# don't worry, as it's also your home
echo "2/3 -> move to ~/xubot-home"
cd ~/xubot-home

# run xubot in the background
# this also allows the ability to close the SSH shell
echo "3/3 -> run xubot"
nohup dotnet xubot-core.dll
