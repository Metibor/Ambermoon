# Monster elements

Each monster has special flags that are stored in a 8 bit value. I am not sure about this value but it looks like the lower 3 bits mark some immunity and the higher 5 bits mark an element.

The following table is not complete yet.

Bit | Property
----|----
0..2 | **Unknown**. Only used by the guard demon who is immune to all kind of damage and spells.
3 | Undead element (all undead)
4 | Stone/earth element (golems)
5 | Wind element (Gargoyle and imp but not minor demon)
6 | Fire element (Demons and fire monsters)
7 | Water element (only the pond lizard)

First I thought that the elements would give several immunities against element spells but this doesn't seem to be the case at all.

## Assumptions

Guard demon has a value 0xff and is immune to all kind of damage. As there is no other monster to my knowledge which is immune to damage I guess the lower 3 bits (which are not used by any other monster) are used for damage immunity. Maybe one bit for close-ranged physical damage, one bit for long-ranged physical damage and one for magical damage. Or one for immunity to all spells as monster knowledge etc won't work either.

But there is another byte (byte 16) which only has a value for the guard demon. Maybe there are the immunities and the 3 bits here represent additional elements which only the guard demon has?

Dark mage (Tar the dark) has a value of 0xf0. So this would mean he has all the 4 major elements besides undead.

## Findings

A petrified enemy gains total immunity against all damage. You cannot attack and damage dealing spells won't do anything. I wasn't able to kill the enemy and had to flee...