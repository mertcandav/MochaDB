//
// MIT License
//
// Copyright (c) 2020 Mertcan Davulcu
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE
// OR OTHER DEALINGS IN THE SOFTWARE.

lexer grammar engine_lexer;

EXTENSION : '.mhdb' ;

// Data Types
DT_STRING : 'String' ;
DT_16BIT_INTEGER : 'Int16' ;
DT_32BIT_INTEGER : 'Int32' ;
DT_64BIT_INTEGER : 'Int64' ;
DT_16BIT_UNSIGNED_INTEGER : 'UInt16' ;
DT_32BIT_UNSIGNED_INTEGER : 'UInt32' ;
DT_64BIT_UNSIGNED_INTEGER : 'UInt64' ;
DT_DOUBLE : 'Double' ;
DT_FLOAT : 'Float' ;
DT_DECIMAL : 'Decimal' ;
DT_BYTE : 'Byte' ;
DT_DATE_TIME : 'DateTime' ;
DT_SIGNED_BYTE : 'SByte' ;
DT_BOOLEAN : 'Boolean' ;
DT_CHAR : 'Char' ;
DT_AUTO_INTEGER : 'AutoInt' ;
DT_UNIQUE : 'Unique' ;
DT_BINARY_DIGIT : 'Bit' ;

// Types
file_type : '1000' ;
interactive_type : '1001' ;
eval_type : '1002' ;
func_type_type : '1003' ;
fstring_type : '1004' ;
type_expressions_type : '1005' ;
statements_type : '1006' ;
statement_type : '1007' ;
statement_newline_type '1008' ;
simple_stmts_type '1009' ;
simple_stmt_type '1010' ;
compound_stmt_type '1011' ;
assignment_type '1012' ; 
augassign_type '1013' ;
global_stmt_type '1014' ;
nonlocal_stmt_type '1015' ;
yield_stmt_type '1016' ;
assert_stmt_type '1017' ;
del_stmt_type '1018' ;
import_stmt_type '1019' ;
import_name_type '1020' ;
import_from_type '1021' ;
import_from_targets_type '1022' ;
import_from_as_names_type '1023' ;
import_from_as_name_type '1024' ;
dotted_as_names_type '1025' ;
dotted_as_name_type '1026' ;
dotted_name_type '1027' ;
if_stmt_type '1028' ;
elif_stmt_type '1029' ;
else_block_type '1030' ;
while_stmt_type '1031' ;
for_stmt_type '1032' ;
with_stmt_type '1033' ;
with_item_type '1034' ;
try_stmt_type '1035' ;
except_block_type '1036' ;
finally_block_type '1037' ;
return_stmt_type '1038' ;
raise_stmt_type '1039' ;
function_def_type '1040' ;
function_def_raw_type '1041' ;
func_type_comment_type '1042' ;
params_type '1043' ;
parameters_type '1044' ;
slash_no_default_type '1045' ;
slash_with_default_type '1046' ;
star_etc_type '1047' ;
kwds_type '1048' ;
param_no_default_type '1049' ;
param_with_default_type '1050' ;
param_maybe_default_type '1051' ;
param_type '1052' ;
annotation_type '1053' ;
default_type '1054' ;
decorators_type '1055' ;
class_def_type '1056' ;
class_def_raw_type '1057' ;
block_type '1058' ;
star_expressions_type '1059' ;
star_expression_type '1060' ;
star_named_expressions_type '1061' ;
star_named_expression_type '1062' ;
named_expression_type '1063' ;
annotated_rhs_type '1064' ;
expressions_type '1065' ;
expression_type '1066' ;
lambdef_type '1067' ;
lambda_params_type '1068' ;
lambda_parameters_type '1069' ;
lambda_slash_no_default_type '1070' ;
lambda_slash_with_default_type '1071' ;
lambda_star_etc_type '1072' ;
lambda_kwds_type '1073' ;
lambda_param_no_default_type '1074' ;
lambda_param_with_default_type '1075' ;
lambda_param_maybe_default_type '1076' ;
lambda_param_type '1077' ;
disjunction_type '1078' ;
conjunction_type '1079' ;
inversion_type '1080' ;
comparison_type '1081' ;
compare_op_bitwise_or_pair_type '1082' ;
eq_bitwise_or_type '1083' ;
noteq_bitwise_or_type '1084' ;
lte_bitwise_or_type '1085' ;
lt_bitwise_or_type '1086' ;
gte_bitwise_or_type '1087' ;
gt_bitwise_or_type '1088' ;
notin_bitwise_or_type '1089' ;
in_bitwise_or_type '1090' ;
isnot_bitwise_or_type '1091' ;
is_bitwise_or_type '1092' ;
bitwise_or_type '1093' ;
bitwise_xor_type '1094' ;
bitwise_and_type '1095' ;
shift_expr_type '1096' ;
sum_type '1097' ;
term_type '1098' ;
factor_type '1099' ;
power_type '1100' ;
await_primary_type '1101' ;
primary_type '1102' ;
slices_type '1103' ;
slice_type '1104' ;
atom_type '1105' ;
strings_type '1106' ;
list_type '1107' ;
listcomp_type '1108' ;
tuple_type '1109' ;
group_type '1110' ;
genexp_type '1111' ;
set_type '1112' ;
setcomp_type '1113' ;
dict_type '1114' ;
dictcomp_type '1115' ;
double_starred_kvpairs_type '1116' ;
double_starred_kvpair_type '1117' ;
kvpair_type '1118' ;
for_if_clauses_type '1119' ;
for_if_clause_type '1120' ;
yield_expr_type '1121' ;
arguments_type '1122' ;
args_type '1123' ;
kwargs_type '1124' ;
starred_expression_type '1125' ;
kwarg_or_starred_type '1126' ;
kwarg_or_double_starred_type '1127' ;
star_targets_type '1128' ;
star_targets_seq_type '1129' ;
star_target_type '1130' ;
star_atom_type '1131' ;
single_target_type '1132' ;
single_subscript_attribute_target_type '1133' ;
del_targets_type '1134' ;
del_target_type '1135' ;
del_t_atom_type '1136' ;
targets_type '1137' ;
target_type '1138' ;
t_primary_type '1139 ' ;
t_lookahead_type '1140' ;
t_atom_type '1141' ;
invalid_arguments_type '1142' ;
invalid_kwarg_type '1143' ;
invalid_named_expression_type '1144' ;
invalid_assignment_type '1145' ;
invalid_ann_assign_target_type '1146' ;
invalid_del_stmt_type '1147' ;
invalid_block_type '1148' ;
invalid_primary_type '1149' ;  
invalid_comprehension_type '1150' ;
invalid_dict_comprehension_type '1151' ;
invalid_parameters_type '1152' ;
invalid_lambda_parameters_type '1153' ;
invalid_star_etc_type '1154' ;
invalid_lambda_star_etc_type '1155' ;
invalid_double_type_comments_type '1156' ;
invalid_with_item_type '1157' ;
invalid_for_target_type '1158' ;
invalid_group_type '1159' ;
invalid_import_from_targets_type '1160' ;
_loop0_1_type '1161' ;
_loop0_2_type '1162' ;
_loop0_4_type '1163' ;
_gather_3_type '1164' ;
_loop0_6_type '1165' ;
_gather_5_type '1166' ;
_loop0_8_type '1167' ;
_gather_7_type '1168' ;
_loop0_10_type '1169' ;
_gather_9_type '1170' ;
_loop1_11_type '1171' ;
_loop0_13_type '1172' ;
_gather_12_type '1173' ;
_tmp_14_type '1174' ;
_tmp_15_type '1175' ;
_tmp_16_type '1176' ;
_tmp_17_type '1177' ;
_tmp_18_type '1178' ;
_tmp_19_type '1179' ;
_tmp_20_type '1180' ;
_tmp_21_type '1181' ;
_loop1_22_type '1182' ;
_tmp_23_type '1183' ;
_tmp_24_type '1184' ;
_loop0_26_type '1185' ;
_gather_25_type '1186' ;
_loop0_28_type '1187' ;
_gather_27_type '1188' ;
_tmp_29_type '1189' ;
_tmp_30_type '1190' ;
_loop0_31_type '1191' ;
_loop1_32_type '1192' ;
_loop0_34_type '1193' ;
_gather_33_type '1194' ;
_tmp_35_type '1195' ;
_loop0_37_type '1196' ;
_gather_36_type '1197' ;
_tmp_38_type '1198' ;
_loop0_40_type '1199' ;
_gather_39_type '1200' ;
_loop0_42_type '1201' ;
_gather_41_type '1202' ;
_loop0_44_type '1203' ;
_gather_43_type '1204' ;
_loop0_46_type '1205' ;
_gather_45_type '1206' ;
_tmp_47_type '1207' ;
_loop1_48_type '1208' ;
_tmp_49_type '1209' ;
_tmp_50_type '1210' ;
_tmp_51_type '1211' ;
_tmp_52_type '1212' ;
_tmp_53_type '1213' ;
_loop0_54_type '1214' ;
_loop0_55_type '1215' ;
_loop0_56_type '1216' ;
_loop1_57_type '1217' ;
_loop0_58_type '1218' ;
_loop1_59_type '1219' ;
_loop1_60_type '1220' ;
_loop1_61_type '1221' ;
_loop0_62_type '1222' ;
_loop1_63_type '1223' ;
_loop0_64_type '1224' ;
_loop1_65_type '1225' ;
_loop0_66_type '1226' ;
_loop1_67_type '1227' ;
_loop1_68_type '1228' ;
_tmp_69_type '1229' ;
_loop1_70_type '1230' ;
_loop0_72_type '1231' ;
_gather_71_type '1232' ;
_loop1_73_type '1233' ;
_loop0_74_type '1234' ;
_loop0_75_type '1235' ;
_loop0_76_type '1236' ;
_loop1_77_type '1237' ;
_loop0_78_type '1238' ;
_loop1_79_type '1239' ;
_loop1_80_type '1240' ;
_loop1_81_type '1241' ;
_loop0_82_type '1242' ;
_loop1_83_type '1243' ;
_loop0_84_type '1244' ;
_loop1_85_type '1245' ;
_loop0_86_type '1246' ;
_loop1_87_type '1247' ;
_loop1_88_type '1248' ;
_loop1_89_type '1249' ;
_loop1_90_type '1250' ;
_tmp_91_type '1251' ;
_loop0_93_type '1252' ;
_gather_92_type '1253' ;
_tmp_94_type '1254' ;
_tmp_95_type '1255' ;
_tmp_96_type '1256' ;
_tmp_97_type '1257' ;
_loop1_98_type '1258' ;
_tmp_99_type '1259' ;
_tmp_100_type '1260' ;
_loop0_102_type '1261' ;
_gather_101_type '1262' ;
_loop1_103_type '1263' ;
_loop0_104_type '1264' ;
_loop0_105_type '1265' ;
_loop0_107_type '1266' ;
_gather_106_type '1267' ;
_tmp_108_type '1268' ;
_loop0_110_type '1269' ;
_gather_109_type '1270' ;
_loop0_112_type '1271' ;
_gather_111_type '1272' ;
_loop0_114_type '1273' ;
_gather_113_type '1274' ;
_loop0_116_type '1275' ;
_gather_115_type '1276' ;
_loop0_117_type '1277' ;
_loop0_119_type '1278' ;
_gather_118_type '1279' ;
_tmp_120_type '1280' ;
_loop0_122_type '1281' ;
_gather_121_type '1282' ;
_loop0_124_type '1283' ;
_gather_123_type '1284' ;
_tmp_125_type '1285' ;
_loop0_126_type '1286' ;
_loop0_127_type '1287' ;
_loop0_128_type '1288' ;
_tmp_129_type '1289' ;
_tmp_130_type '1290' ;
_loop0_131_type '1291' ;
_tmp_132_type '1292' ;
_loop0_133_type '1293' ;
_tmp_134_type '1294' ;
_tmp_135_type '1295' ;
_tmp_136_type '1296' ;
_tmp_137_type '1297' ;
_tmp_138_type '1298' ;
_tmp_139_type '1299' ;
_tmp_140_type '1300' ;
_tmp_141_type '1301' ;
_tmp_142_type '1302' ;
_tmp_143_type '1303' ;
_tmp_144_type '1304' ;
_tmp_145_type '1305' ;
_tmp_146_type '1306' ;
_tmp_147_type '1307' ;
_tmp_148_type '1308' ;
_tmp_149_type '1309' ;
_tmp_150_type '1310' ;
_loop1_151_type '1311' ;
_loop1_152_type '1312' ;
_tmp_153_type '1313' ;
_tmp_154_type '1314' ;
