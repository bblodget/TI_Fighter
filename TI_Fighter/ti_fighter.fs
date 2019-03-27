FORGET -->>

: -->> 0 EMIT [COMPILE] --> ; IMMEDIATE

: Reload ( --)
    \ Reloads FIGHTER block file
    S" DSK3.FIGHTER" USE
    ;


   0 VALUE Column			\ used by DrawIt
   0 VALUE Row			\ used by DrawIt
   1 CONSTANT Fire?		\ comparison check for fire button
   2 CONSTANT Left?		\ comparison check for left
   4 CONSTANT Right?		\ comparison check for right
   8 CONSTANT Down?		\ comparison check for down
  16 CONSTANT Up?			\ comparison check for up
  13 CONSTANT ENTER		\ key code for ENTER key
 400 CONSTANT DelayTime	\ delay loop count

HEX
\ user defined graphics
: BrickUDG ( --) \ character data for brick wall
   DATA 4 7FAA D5A8 D0A0 C080 80 DCHAR
   DATA 4 8080 8080 8080 807F 81 DCHAR
   DATA 4 FEA9 4101 0101 0101 82 DCHAR
   DATA 4 0101 0101 0101 01FE 83 DCHAR ;

: LLineUDG ( --) \ Vertical line on left side
   DATA 4 8080 8080 8080 8080 88 DCHAR ;

: RLineUDG ( --) \ Vertical line on right side
   DATA 4 0101 0101 0101 0101 89 DCHAR ;

: BLineUDG ( --) \ Horizontal line on bottom side
   DATA 4 0000 0000 0000 00FF 8A DCHAR ;

: TLineUDG ( --) \ Horizontal line on top side
   DATA 4 FF00 0000 0000 0000 8B DCHAR ;

: LBLineUDG ( --) \ Lines on left and bottom
   DATA 4 8080 8080 8080 80FF 8C DCHAR ;

: RBLineUDG ( --) \ Lines on right and bottom
   DATA 4 0101 0101 0101 01FF 8D DCHAR ;

: LTLineUDG ( --) \ Lines on left and top
   DATA 4 FF80 8080 8080 8080 8E DCHAR ;

: RTLineUDG ( --) \ Lines on right and top
   DATA 4 FF01 0101 0101 0101 8F DCHAR ;

: LBTLineUDG ( --) \ Lines on left, bottom and top
   DATA 4 FF80 8080 8080 80FF 90 DCHAR ;

: RBTLineUDG ( --) \ Lines on right, bottom and top
   DATA 4 FF01 0101 0101 01FF 91 DCHAR ;

: BTLineUDG ( --) \ Lines on bottom and top
   DATA 4 FF00 0000 0000 00FF 92 DCHAR ;

: DiagUpUDG ( --) \ Diagonal from lower left to upper right
   DATA 4 FFFE FCF8 F0E0 C080 98 DCHAR ;

: DiagDownUDG ( --) \ Diagonal from upper right to lower left
   DATA 4 FF7F 3F1F 0F07 0301 99 DCHAR ;

: ManRight1 ( --) \ man facing right, frame #1
   DATA 4 0302 0201 0302 0607 100 DCHAR
   DATA 4 0603 0202 0202 0203 101 DCHAR
   DATA 4 0080 8000 C048 6850 102 DCHAR
   DATA 4 40C0 4040 4040 4070 103 DCHAR ;

: ManRight2 ( --) \ man facing right, frame #2
   DATA 4 0101 0100 0306 0A07 100 DCHAR
   DATA 4 0203 0202 0204 0406 101 DCHAR
   DATA 4 8060 6080 C868 5040 102 DCHAR
   DATA 4 40C0 4020 1010 101C 103 DCHAR ;

DECIMAL

: NextColumn ( --)	\ advance to the next column
   2 +TO Column ;

: DrawIt ( a b c d --)
  \ emits the four ascii characters on the stack to the screen as follows:
  \ ac
  \ bd
   Column 1+ Row 1+ GOTOXY EMIT \ d
   Column 1+ Row GOTOXY EMIT    \ c
   Column Row 1+ GOTOXY EMIT    \ b
   Column Row GOTOXY EMIT       \ a
   NextColumn ;

: DrawBrick ( --) \ draws a brick tile at Column & Row
   128 129 130 131 DrawIt ;

: ManSprite ( --) \  Defines sprite 0
    0 128 48 0 5 SPRITE ;

: DefineGraphics ( --)
  \ define the user defined graphics and set colours of character sets
   1 GMODE FALSE SSCROLL !
   32 0 DO I 1 14 COLOR LOOP
   16 1 15 COLOR  	\ brick colour
   17 1 8  COLOR  	\ shrine color
   18 1 8  COLOR  	\ shrine color
   19 8 14  COLOR  	\ shrine corner color
   BrickUDG 
   LLineUDG RLineUDG BLineUDG TLineUDG
   LBLineUDG RBLineUDG LTLineUDG RTLineUDG
   LBTLineUDG RBTLineUDG BTLineUDG 
   DiagUpUDG DiagDownUDG
   ManRight1
   ;

: BrickRows ( --)
    \ draws rows of bricks at top and bottom of screen
    0 TO Column
    20 TO Row
    2 0 DO
    16 0 DO DrawBrick LOOP
       2 +TO Row 0 TO Column
    LOOP
    ;

: ShintoShrine ( x y --) \ (x,y) upper left corner of shrine
    \ draws the shrine
    TO Column
    TO Row

    \ Top line of big beam
    Column Row GOTOXY 142 EMIT
    15 1 DO Column I + Row GOTOXY 139 EMIT LOOP
    Column 15 + Row GOTOXY 143 EMIT

    \ Bottom line of big beam
    1 +TO Row
    Column Row GOTOXY 153 EMIT 
    15 1 DO Column I + Row GOTOXY 138 EMIT LOOP
    Column 15 + Row GOTOXY 152 EMIT 

    \ Left Beam
    1 +TO Row
    3 +TO Column
    9 0 DO Column Row I + GOTOXY 136 EMIT 137 EMIT LOOP

    \ Right Beam
    8 +TO Column
    9 0 DO Column Row I + GOTOXY 136 EMIT 137 EMIT LOOP

    \ Small beam
    -10 +TO Column
    1 +TO Row
    Column Row GOTOXY 144 EMIT
    13 1 DO Column I + Row GOTOXY 146 EMIT LOOP
    Column 13 + Row GOTOXY 145 EMIT
    ;

: Fighter ( --)
    \ entry point of game
    DefineGraphics BrickRows
    9 7 ShintoShrine
    3 MAGNIFY
    ManSprite
    30 23 GOTOXY KEY DROP ;

