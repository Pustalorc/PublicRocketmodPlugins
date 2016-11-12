----------- Rocket Rules -----------

Version: Public 2.0.0.1

[![Build status](https://ci.appveyor.com/api/projects/status/pb6sp9f67e4dj9is?svg=true)](https://ci.appveyor.com/project/persiafighter/rocket-rules)

Last Update: November 8, 2016

--

#Permissions

        <Permission Cooldown="0">rules</Permission>

#Translation file

        <?xml version="1.0" encoding="utf-8"?>
	<Translations xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
	  <Translation Id="rule" Value="{0}" />
	  <Translation Id="pages" Value="Next page: &quot;/rules {0}&quot;." />
	  <Translation Id="endofrules" Value="You have reached the end of the rules." />
	</Translations>

#Config file

        <?xml version="1.0" encoding="utf-8"?>
	<RocketRulesConfiguration xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
	  <DisplayOnConnect>false</DisplayOnConnect>
	  <Rules>
	    <Rule>
	      <configText>Example Rule 1</configText>
	      <configColor>
	        <r>1</r>
	        <g>0.921568632</g>
	        <b>0.0156862754</b>
	        <a>1</a>
	      </configColor>
	    </Rule>
	    <Rule>
	      <configText>Example Rule 2</configText>
	      <configColor>
	        <r>0</r>
	        <g>1</g>
	        <b>0</b>
	        <a>1</a>
	      </configColor>
	    </Rule>
	    <Rule>
	      <configText>Example Rule 3</configText>
	      <configColor>
	        <r>1</r>
	        <g>0</g>
	        <b>0</b>
	        <a>1</a>
	      </configColor>
	    </Rule>
	    <Rule>
	      <configText>Example Rule 4</configText>
	      <configColor>
	        <r>0</r>
	        <g>0</g>
	        <b>1</b>
	        <a>1</a>
	      </configColor>
	    </Rule>
	  </Rules>
	</RocketRulesConfiguration>

#Download:

https://github.com/persiafighter/Rocket-Rules/raw/master/bin/Release/RocketRules.dll

https://dev.rocketmod.net/plugins/rules/

#Changelog:

**V2.0.0.1** - 

* Fixed error for the permissions for the command
* Removed temporary files/non used files

**V2.0.0.0** - 

* Increased ammount of rules. Pasting multiple more <Rule>...</Rule> settings allows for more rules.
* Added paging for when there is more than 3 rules.
* Added more notifications.
* Simplified translations.
* Fixed some minor errors (including command execution + syntax)

**V1.0.1.0** -

* Added option to enable display of rules in chat on player connect.
* Added logging of current settings in console.
* Changed console logging color.

**V1.0.0.0** - Created the plugin.

----------- Rocket Rules -----------
