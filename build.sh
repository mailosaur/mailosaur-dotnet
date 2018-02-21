#!/bin/sh

# Clean output directory, excluding customised files
keep="Mailosaur/bin Mailosaur/obj Mailosaur/Mailosaur.csproj Mailosaur/Mailosaur.nuspec Mailosaur/MailosaurExtensions.cs Mailosaur/packages.config"

for f in Mailosaur/*
do
  if [[ ! " ${keep[@]} " =~ " ${f} " ]]; then
    rm -rf "$f"
  fi
done

# Rebuild generated code
autorest

# Rename MailosaurErrorException to MailosaurException
# Rename FromProperty to From (autorest avoids using From by default)
for f in `find Mailosaur -type f -name "*.cs"`
do
  sed -i "" s/MailosaurErrorException/MailosaurException/g "$f"
  sed -i "" s/FromProperty/From/g "$f"
done

# Update references to MailosaurErrorException
mv Mailosaur/Models/MailosaurErrorException.cs Mailosaur/Models/MailosaurException.cs
sed -i "" "s/MailosaurError\ Body\ {\ get/MailosaurError Body { private get/" Mailosaur/Models/MailosaurException.cs
sed -i '' '/MailosaurError\ Body/ a\
  public MailosaurError MailosaurError => Body;\
  ' Mailosaur/Models/MailosaurException.cs

# Update dependencies and rebuild project
dotnet restore Mailosaur
dotnet build Mailosaur