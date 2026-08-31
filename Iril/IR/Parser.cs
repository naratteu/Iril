// created by jay 0.7 (c) 1998 Axel.Schreiner@informatik.uni-osnabrueck.de

#line 2 "Iril/IR/IR.jay"
using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text;
using System.Linq;

using Iril.Types;

#pragma warning disable 219,414

namespace Iril.IR
{
	public partial class Parser
	{
#line default

  /** error output stream.
      It should be changeable.
    */
  public System.IO.TextWriter ErrorOutput = new StringWriter ();

  /** simplified error message.
      @see <a href="#yyerror(java.lang.String, java.lang.String[])">yyerror</a>
    */
  public void yyerror (string message) {
    yyerror(message, null);
  }

  /* An EOF token */
  public int eof_token;
  
  public int yacc_verbose_flag;

  /** (syntax) error message.
      Can be overwritten to control message format.
      @param message text to be displayed.
      @param expected vector of acceptable tokens, if available.
    */
  public void yyerror (string message, string[] expected) {
    if ((yacc_verbose_flag > 0) && (expected != null) && (expected.Length  > 0)) {
      ErrorOutput.Write (message+", expecting");
      for (int n = 0; n < expected.Length; ++ n)
        ErrorOutput.Write (" "+expected[n]);
        ErrorOutput.WriteLine ();
    } else
      ErrorOutput.WriteLine (message);
  }

  /** debugging support, requires the package jay.yydebug.
      Set to null to suppress debugging messages.
    */
//t  internal yydebug.yyDebug debug;

  protected const int yyFinal = 9;
//t // Put this array into a separate class so it is only initialized if debugging is actually used
//t // Use MarshalByRefObject to disable inlining
//t class YYRules : MarshalByRefObject {
//t  public static readonly string [] yyRule = {
//t    "$accept : module",
//t    "module : module_parts",
//t    "module_parts : module_part",
//t    "module_parts : module_parts module_part",
//t    "module_part : SOURCE_FILENAME '=' STRING",
//t    "module_part : TARGET DATALAYOUT '=' STRING",
//t    "module_part : TARGET TRIPLE '=' STRING",
//t    "module_part : LOCAL_SYMBOL '=' TYPE literal_structure",
//t    "module_part : LOCAL_SYMBOL '=' TYPE OPAQUE",
//t    "module_part : function_definition",
//t    "module_part : function_declaration",
//t    "module_part : global_variable",
//t    "module_part : ATTRIBUTES ATTRIBUTE_GROUP_REF '=' '{' attributes '}'",
//t    "module_part : ATTRIBUTES ATTRIBUTE_GROUP_REF '=' '{' '}'",
//t    "module_part : META_SYMBOL_DEF '=' '!' '{' '}'",
//t    "module_part : META_SYMBOL_DEF '=' '!' '{' metadata '}'",
//t    "module_part : META_SYMBOL_DEF '=' META_SYMBOL '(' metadata_args ')'",
//t    "module_part : META_SYMBOL_DEF '=' DISTINCT '!' '{' metadata '}'",
//t    "module_part : META_SYMBOL_DEF '=' DISTINCT META_SYMBOL '(' metadata_args ')'",
//t    "global_variable : GLOBAL_SYMBOL '=' linkage global_kind type value",
//t    "global_variable : GLOBAL_SYMBOL '=' external_linkage global_kind type",
//t    "global_variable : GLOBAL_SYMBOL '=' external_linkage global_kind type ',' ALIGN INTEGER",
//t    "global_variable : GLOBAL_SYMBOL '=' external_linkage function_addr global_kind type",
//t    "global_variable : GLOBAL_SYMBOL '=' external_linkage function_addr global_kind type ',' ALIGN INTEGER",
//t    "global_variable : GLOBAL_SYMBOL '=' external_linkage visibility_style global_kind type",
//t    "global_variable : GLOBAL_SYMBOL '=' linkage visibility_style global_kind type",
//t    "global_variable : GLOBAL_SYMBOL '=' linkage global_kind type value ',' SECTION STRING",
//t    "global_variable : GLOBAL_SYMBOL '=' linkage function_addr global_kind type value ',' SECTION STRING",
//t    "global_variable : GLOBAL_SYMBOL '=' linkage function_addr global_kind type value ',' SECTION STRING ',' ALIGN INTEGER",
//t    "global_variable : GLOBAL_SYMBOL '=' global_kind type value ',' ALIGN INTEGER metadata_kvs",
//t    "global_variable : GLOBAL_SYMBOL '=' function_addr global_kind type value ',' ALIGN INTEGER metadata_kvs",
//t    "global_variable : GLOBAL_SYMBOL '=' visibility_style function_addr global_kind type value ',' ALIGN INTEGER",
//t    "global_variable : GLOBAL_SYMBOL '=' visibility_style global_kind type value ',' ALIGN INTEGER metadata_kvs",
//t    "global_variable : GLOBAL_SYMBOL '=' visibility function_addr global_kind type value ',' ALIGN INTEGER",
//t    "global_variable : GLOBAL_SYMBOL '=' visibility function_addr global_kind type value",
//t    "global_variable : GLOBAL_SYMBOL '=' visibility function_addr global_kind type value ',' ALIGN INTEGER metadata_kvs",
//t    "global_variable : GLOBAL_SYMBOL '=' visibility global_kind type value",
//t    "global_variable : GLOBAL_SYMBOL '=' visibility global_kind type value ',' ALIGN INTEGER",
//t    "global_variable : GLOBAL_SYMBOL '=' visibility global_kind type value ',' SECTION STRING",
//t    "global_variable : GLOBAL_SYMBOL '=' visibility global_kind type value ',' SECTION STRING ',' ALIGN INTEGER",
//t    "global_variable : GLOBAL_SYMBOL '=' linkage function_addr global_kind type ',' ALIGN INTEGER",
//t    "global_variable : GLOBAL_SYMBOL '=' linkage function_addr global_kind type value ',' ALIGN INTEGER",
//t    "global_variable : GLOBAL_SYMBOL '=' linkage function_addr global_kind type value ',' ALIGN INTEGER metadata_kvs",
//t    "global_variable : GLOBAL_SYMBOL '=' linkage global_kind type value ',' ALIGN INTEGER",
//t    "global_variable : GLOBAL_SYMBOL '=' linkage global_kind type value ',' ALIGN INTEGER metadata_kvs",
//t    "global_variable : GLOBAL_SYMBOL '=' linkage global_kind type ',' ALIGN INTEGER",
//t    "global_variable : GLOBAL_SYMBOL '=' linkage global_kind type",
//t    "global_kind : GLOBAL",
//t    "global_kind : CONSTANT",
//t    "external_linkage : EXTERNAL",
//t    "linkage : AVAILABLE_EXTERNALLY",
//t    "linkage : INTERNAL",
//t    "linkage : LINKONCE",
//t    "linkage : LINKONCE_ODR",
//t    "linkage : WEAK",
//t    "linkage : WEAK_ODR",
//t    "linkage : APPENDING",
//t    "linkage : COMMON",
//t    "visibility : PRIVATE",
//t    "visibility_style : HIDDEN",
//t    "metadata_args : metadata_arg",
//t    "metadata_args : metadata_args ',' metadata_arg",
//t    "metadata_arg : SYMBOL ':' metadata_arg_expr",
//t    "metadata_arg : TYPE ':' metadata_arg_expr",
//t    "metadata_arg : ALIGN ':' constant",
//t    "metadata_arg : SYMBOL ':' META_SYMBOL '(' metadata_value_args ')'",
//t    "metadata_arg : SYMBOL ':' META_SYMBOL '(' ')'",
//t    "metadata_arg_expr : metadata_arg_or_expr",
//t    "metadata_arg_or_expr : metadata_arg_and_expr",
//t    "metadata_arg_or_expr : metadata_arg_or_expr '|' metadata_arg_and_expr",
//t    "metadata_arg_and_expr : metadata_arg_primary",
//t    "metadata_arg_primary : SYMBOL",
//t    "metadata_arg_primary : META_SYMBOL",
//t    "metadata_arg_primary : STRING",
//t    "metadata_arg_primary : constant",
//t    "metadata_arg_primary : typed_constant",
//t    "metadata_arg_primary : NULL",
//t    "metadata_kvs : META_SYMBOL META_SYMBOL",
//t    "metadata_kvs : metadata_kvs META_SYMBOL META_SYMBOL",
//t    "metadata : metadatum",
//t    "metadata : metadata META_SYMBOL",
//t    "metadata : metadata ',' typed_value",
//t    "metadata : metadata ',' META_SYMBOL",
//t    "metadata : metadata ',' NULL",
//t    "metadatum : typed_value",
//t    "metadatum : META_SYMBOL",
//t    "metadatum : NULL",
//t    "attributes : attribute",
//t    "attributes : attributes attribute",
//t    "attribute : NORECURSE",
//t    "attribute : NOUNWIND",
//t    "attribute : READNONE",
//t    "attribute : SPECULATABLE",
//t    "attribute : SSP",
//t    "attribute : UWTABLE",
//t    "attribute : ARGMEMONLY",
//t    "attribute : STRING '=' STRING",
//t    "attribute : STRING",
//t    "attribute : SYMBOL",
//t    "attribute : READONLY",
//t    "attribute : WRITEONLY",
//t    "attribute : SYMBOL '(' metadata_value_args ')'",
//t    "literal_structure : '{' '}'",
//t    "literal_structure : '{' type_list '}'",
//t    "literal_structure : '<' '{' type_list '}' '>'",
//t    "type_list : type",
//t    "type_list : type_list ',' type",
//t    "return_type : type",
//t    "return_type : VOID",
//t    "type : literal_structure",
//t    "type : INTEGER_TYPE",
//t    "type : HALF",
//t    "type : FLOAT",
//t    "type : DOUBLE",
//t    "type : X86_FP80",
//t    "type : return_type '(' ')'",
//t    "type : return_type '(' function_type_args ')'",
//t    "type : type optional_addrspace '*'",
//t    "type : type optional_addrspace '*' ALIGN INTEGER",
//t    "type : LOCAL_SYMBOL",
//t    "type : PTR optional_addrspace",
//t    "type : '<' INTEGER X type '>'",
//t    "type : '[' INTEGER X type ']'",
//t    "addrspace : ADDRSPACE '(' INTEGER ')'",
//t    "optional_addrspace :",
//t    "optional_addrspace : ADDRSPACE '(' INTEGER ')'",
//t    "function_type_args : function_type_arg",
//t    "function_type_args : function_type_args ',' function_type_arg",
//t    "function_type_arg : type",
//t    "function_type_arg : ELLIPSIS",
//t    "function_definition : define_header GLOBAL_SYMBOL parameters define_tail '{' blocks '}'",
//t    "function_definition : define_header GLOBAL_SYMBOL parameters define_tail metadata_kvs '{' blocks '}'",
//t    "define_tail : function_addr",
//t    "define_tail : function_addr attribute_group_refs",
//t    "define_tail : function_addr attribute_group_refs ALIGN INTEGER",
//t    "define_tail : function_addr attribute_group_refs personality_function",
//t    "define_tail : function_addr attribute_group_refs ALIGN INTEGER personality_function",
//t    "define_tail : function_addr attribute_group_refs SECTION STRING",
//t    "define_tail : attribute_group_refs",
//t    "define_tail : attribute_group_refs ALIGN INTEGER",
//t    "define_tail : attribute_group_refs personality_function",
//t    "define_tail : attribute_group_refs ALIGN INTEGER personality_function",
//t    "define_tail : attribute_group_refs SECTION STRING",
//t    "define_header : DEFINE return_type",
//t    "define_header : DEFINE parameter_attribute return_type",
//t    "define_header : DEFINE define_header_attributes return_type",
//t    "define_header : DEFINE define_header_attributes visibility_style return_type",
//t    "define_header : DEFINE define_header_attributes visibility_style calling_convention return_type",
//t    "define_header : DEFINE define_header_attributes parameter_attributes return_type",
//t    "define_header_attributes : NOALIAS",
//t    "define_header_attributes : runtime_preemption_specifier",
//t    "define_header_attributes : calling_convention",
//t    "define_header_attributes : linkage",
//t    "define_header_attributes : linkage runtime_preemption_specifier",
//t    "define_header_attributes : linkage runtime_preemption_specifier calling_convention",
//t    "define_header_attributes : linkage calling_convention",
//t    "personality_function : PERSONALITY typed_value",
//t    "declare_head : DECLARE",
//t    "declare_head : DECLARE metadata_kvs",
//t    "function_declaration : declare_head return_type GLOBAL_SYMBOL parameters",
//t    "function_declaration : declare_head return_type GLOBAL_SYMBOL parameters declare_tail",
//t    "function_declaration : declare_head NOALIAS return_type GLOBAL_SYMBOL parameters declare_tail",
//t    "function_declaration : declare_head parameter_attributes return_type GLOBAL_SYMBOL parameters declare_tail",
//t    "function_declaration : declare_head NOALIAS parameter_attributes return_type GLOBAL_SYMBOL parameters declare_tail",
//t    "function_declaration : declare_head calling_convention return_type GLOBAL_SYMBOL parameters",
//t    "function_declaration : declare_head calling_convention return_type GLOBAL_SYMBOL parameters declare_tail",
//t    "function_declaration : declare_head calling_convention parameter_attributes return_type GLOBAL_SYMBOL parameters declare_tail",
//t    "declare_tail : function_addr",
//t    "declare_tail : function_addr attribute_group_refs",
//t    "declare_tail : attribute_group_refs",
//t    "parameters : '(' parameter_list ')'",
//t    "parameters : '(' ')'",
//t    "parameter_list : parameter",
//t    "parameter_list : parameter_list ',' parameter",
//t    "parameter : type",
//t    "parameter : type LOCAL_SYMBOL",
//t    "parameter : type parameter_attributes",
//t    "parameter : type parameter_attributes LOCAL_SYMBOL",
//t    "parameter : METADATA",
//t    "parameter : ELLIPSIS",
//t    "parameter_attributes : parameter_attribute",
//t    "parameter_attributes : parameter_attributes parameter_attribute",
//t    "parameter_attribute : NONNULL",
//t    "parameter_attribute : NOCAPTURE",
//t    "parameter_attribute : CAPTURES '(' capture_components ')'",
//t    "parameter_attribute : SWIFTSELF",
//t    "parameter_attribute : SWIFTERROR",
//t    "parameter_attribute : SWIFTASYNC",
//t    "parameter_attribute : NOUNDEF",
//t    "parameter_attribute : IMMARG",
//t    "parameter_attribute : READONLY",
//t    "parameter_attribute : WRITEONLY",
//t    "parameter_attribute : READNONE",
//t    "parameter_attribute : SIGNEXT",
//t    "parameter_attribute : ZEROEXT",
//t    "parameter_attribute : RETURNED",
//t    "parameter_attribute : SRET",
//t    "parameter_attribute : SRET '(' type ')'",
//t    "parameter_attribute : NOALIAS",
//t    "parameter_attribute : BYVAL",
//t    "parameter_attribute : BYVAL '(' type ')'",
//t    "parameter_attribute : DEREFERENCEABLE '(' INTEGER ')'",
//t    "parameter_attribute : ALIGN INTEGER",
//t    "capture_components : SYMBOL",
//t    "capture_components : capture_components ',' SYMBOL",
//t    "function_addr_type : UNNAMED_ADDR",
//t    "function_addr_type : LOCAL_UNNAMED_ADDR",
//t    "function_addr : function_addr_type optional_addrspace",
//t    "function_addr : function_addr_type optional_addrspace EXTERNALLY_INITIALIZED",
//t    "function_addr : optional_addrspace EXTERNALLY_INITIALIZED",
//t    "function_addr : addrspace",
//t    "runtime_preemption_specifier : DSO_LOCAL",
//t    "runtime_preemption_specifier : DSO_PREEMPTABLE",
//t    "attribute_group_refs : attribute_group_ref",
//t    "attribute_group_refs : attribute_group_refs attribute_group_ref",
//t    "attribute_group_ref : ATTRIBUTE_GROUP_REF",
//t    "icmp_condition : EQ",
//t    "icmp_condition : NE",
//t    "icmp_condition : UGT",
//t    "icmp_condition : UGE",
//t    "icmp_condition : ULT",
//t    "icmp_condition : ULE",
//t    "icmp_condition : SGT",
//t    "icmp_condition : SGE",
//t    "icmp_condition : SLT",
//t    "icmp_condition : SLE",
//t    "fcmp_condition : TRUE",
//t    "fcmp_condition : FALSE",
//t    "fcmp_condition : ORD",
//t    "fcmp_condition : OEQ",
//t    "fcmp_condition : ONE",
//t    "fcmp_condition : OGT",
//t    "fcmp_condition : OGE",
//t    "fcmp_condition : OLT",
//t    "fcmp_condition : OLE",
//t    "fcmp_condition : UNO",
//t    "fcmp_condition : UEQ",
//t    "fcmp_condition : UNE",
//t    "fcmp_condition : UGT",
//t    "fcmp_condition : UGE",
//t    "fcmp_condition : ULT",
//t    "fcmp_condition : ULE",
//t    "global_value : GLOBAL_SYMBOL",
//t    "value : global_value",
//t    "value : nonglobal_value",
//t    "nonglobal_value : constant",
//t    "nonglobal_value : LOCAL_SYMBOL",
//t    "nonglobal_value : INTTOPTR '(' typed_value TO type ')'",
//t    "nonglobal_value : GETELEMENTPTR INBOUNDS '(' type ',' typed_value ',' element_indices ')'",
//t    "nonglobal_value : GETELEMENTPTR INBOUNDS NUW '(' type ',' typed_value ',' element_indices ')'",
//t    "nonglobal_value : GETELEMENTPTR '(' type ',' typed_value ',' element_indices ')'",
//t    "nonglobal_value : BITCAST '(' typed_value TO type ')'",
//t    "nonglobal_value : PTRTOINT '(' typed_value TO type ')'",
//t    "nonglobal_value : '<' typed_values '>'",
//t    "nonglobal_value : '[' typed_values ']'",
//t    "nonglobal_value : '{' typed_values '}'",
//t    "nonglobal_value : '<' '{' typed_values '}' '>'",
//t    "nonglobal_value : ADDRSPACECAST '(' typed_value TO type ')'",
//t    "pointer_value : value",
//t    "constant : NULL",
//t    "constant : FLOAT_LITERAL",
//t    "constant : INTEGER",
//t    "constant : HEX_INTEGER",
//t    "constant : TRUE",
//t    "constant : FALSE",
//t    "constant : UNDEF",
//t    "constant : ZEROINITIALIZER",
//t    "constant : CONSTANT_BYTES",
//t    "label_value : LABEL LOCAL_SYMBOL",
//t    "typed_value : type value",
//t    "typed_value : VOID",
//t    "typed_pointer_value : type pointer_value",
//t    "typed_values : typed_value",
//t    "typed_values : typed_values ',' typed_value",
//t    "typed_constant : type constant",
//t    "element_index : typed_value",
//t    "element_index : INRANGE typed_value",
//t    "element_indices : element_index",
//t    "element_indices : element_indices ',' element_index",
//t    "index : constant",
//t    "indices : index",
//t    "indices : indices ',' index",
//t    "blocks : lblock",
//t    "blocks : blocks lblock",
//t    "lblock : INTEGER ':' block",
//t    "lblock : SYMBOL ':' block",
//t    "lblock : block",
//t    "block : assignments terminator_assignment",
//t    "block : assignments terminator_assignment metadata_kvs",
//t    "block : terminator_assignment",
//t    "block : terminator_assignment metadata_kvs",
//t    "assignments : assignment",
//t    "assignments : assignments assignment",
//t    "assignment : instruction",
//t    "assignment : instruction metadata_kvs",
//t    "assignment : LOCAL_SYMBOL '=' instruction",
//t    "assignment : LOCAL_SYMBOL '=' instruction metadata_kvs",
//t    "function_pointer : value",
//t    "function_args : '(' function_arg_list ')'",
//t    "function_args : '(' ')'",
//t    "function_arg_list : function_arg",
//t    "function_arg_list : function_arg_list ',' function_arg",
//t    "function_arg : type value",
//t    "function_arg : type parameter_attributes value",
//t    "function_arg : METADATA type metadata_value",
//t    "function_arg : METADATA META_SYMBOL",
//t    "function_arg : METADATA META_SYMBOL '(' ')'",
//t    "function_arg : METADATA META_SYMBOL '(' metadata_value_args ')'",
//t    "metadata_value : constant",
//t    "metadata_value : GLOBAL_SYMBOL",
//t    "metadata_value : LOCAL_SYMBOL",
//t    "metadata_value : SYMBOL",
//t    "metadata_value : INTTOPTR '(' typed_value TO type ')'",
//t    "metadata_value : GETELEMENTPTR INBOUNDS '(' type ',' typed_value ',' element_indices ')'",
//t    "metadata_value : GETELEMENTPTR INBOUNDS NUW '(' type ',' typed_value ',' element_indices ')'",
//t    "metadata_value : BITCAST '(' typed_value TO type ')'",
//t    "metadata_value : PTRTOINT '(' typed_value TO type ')'",
//t    "metadata_value_args : metadata_value_arg",
//t    "metadata_value_args : metadata_value_args ',' metadata_value_arg",
//t    "metadata_value_arg : constant",
//t    "metadata_value_arg : SYMBOL",
//t    "metadata_value_arg : type LOCAL_SYMBOL",
//t    "metadata_value_arg : type SYMBOL",
//t    "metadata_value_arg : type UNDEF",
//t    "phi_vals : phi_val",
//t    "phi_vals : phi_vals ',' phi_val",
//t    "phi_val : '[' value ',' value ']'",
//t    "switch_cases : switch_case",
//t    "switch_cases : switch_cases switch_case",
//t    "switch_case : typed_constant ',' label_value",
//t    "wrappings : wrapping",
//t    "wrappings : wrappings wrapping",
//t    "wrapping : NUW",
//t    "wrapping : NSW",
//t    "calling_convention : FASTCC",
//t    "calling_convention : SWIFTCC",
//t    "calling_convention : SWIFTTAILCC",
//t    "atomic_constraint : SEQ_CST",
//t    "inline_assembly : ASM SIDEEFFECT STRING ',' STRING",
//t    "terminator_assignment : terminator_instruction",
//t    "terminator_assignment : LOCAL_SYMBOL '=' invoke_instruction",
//t    "terminator_instruction : BR label_value",
//t    "terminator_instruction : BR INTEGER_TYPE value ',' label_value ',' label_value",
//t    "terminator_instruction : RESUME typed_value",
//t    "terminator_instruction : RET typed_value",
//t    "terminator_instruction : SWITCH typed_value ',' label_value '[' switch_cases ']'",
//t    "terminator_instruction : UNREACHABLE",
//t    "terminator_instruction : invoke_instruction",
//t    "invoke_instruction : INVOKE return_type function_pointer function_args TO label_value UNWIND label_value",
//t    "invoke_instruction : INVOKE parameter_attributes return_type function_pointer function_args TO label_value UNWIND label_value",
//t    "invoke_instruction : INVOKE calling_convention return_type function_pointer function_args TO label_value UNWIND label_value",
//t    "invoke_instruction : INVOKE calling_convention parameter_attributes return_type function_pointer function_args TO label_value UNWIND label_value",
//t    "optional_fast :",
//t    "optional_fast : FAST",
//t    "instruction : ADD type value ',' value",
//t    "instruction : ADD wrappings type value ',' value",
//t    "instruction : ATOMICRMW ADD type value ',' type value SEQ_CST ',' ALIGN INTEGER",
//t    "instruction : ALLOCA type ',' ALIGN INTEGER",
//t    "instruction : ALLOCA type ',' typed_value ',' ALIGN INTEGER",
//t    "instruction : AND type value ',' value",
//t    "instruction : ASHR type value ',' value",
//t    "instruction : ASHR EXACT type value ',' value",
//t    "instruction : BITCAST typed_value TO type",
//t    "instruction : CALL return_type function_pointer function_args",
//t    "instruction : CALL return_type inline_assembly function_args attribute_group_refs",
//t    "instruction : CALL calling_convention return_type function_pointer function_args",
//t    "instruction : CALL calling_convention return_type function_pointer function_args attribute_group_refs",
//t    "instruction : CALL calling_convention parameter_attribute return_type function_pointer function_args",
//t    "instruction : CALL calling_convention parameter_attribute return_type function_pointer function_args attribute_group_refs",
//t    "instruction : CALL return_type function_pointer function_args attribute_group_refs",
//t    "instruction : CALL parameter_attribute return_type function_pointer function_args",
//t    "instruction : CALL parameter_attribute return_type function_pointer function_args attribute_group_refs",
//t    "instruction : TAIL CALL return_type function_pointer function_args attribute_group_refs",
//t    "instruction : TAIL CALL FAST return_type function_pointer function_args attribute_group_refs",
//t    "instruction : TAIL CALL parameter_attribute return_type function_pointer function_args",
//t    "instruction : TAIL CALL parameter_attribute return_type function_pointer function_args attribute_group_refs",
//t    "instruction : TAIL CALL return_type function_pointer function_args",
//t    "instruction : TAIL CALL calling_convention return_type function_pointer function_args",
//t    "instruction : TAIL CALL calling_convention return_type function_pointer function_args attribute_group_refs",
//t    "instruction : TAIL CALL calling_convention parameter_attributes return_type function_pointer function_args",
//t    "instruction : TAIL CALL calling_convention parameter_attributes return_type function_pointer function_args attribute_group_refs",
//t    "instruction : TAIL CALL return_type inline_assembly function_args attribute_group_refs",
//t    "instruction : EXTRACTELEMENT typed_value ',' typed_value",
//t    "instruction : EXTRACTVALUE typed_value ',' indices",
//t    "instruction : FADD optional_fast type value ',' value",
//t    "instruction : FCMP optional_fast fcmp_condition type value ',' value",
//t    "instruction : FDIV optional_fast type value ',' value",
//t    "instruction : FENCE atomic_constraint",
//t    "instruction : FMUL optional_fast type value ',' value",
//t    "instruction : FPEXT typed_value TO type",
//t    "instruction : FPTOUI typed_value TO type",
//t    "instruction : FPTOSI typed_value TO type",
//t    "instruction : FPTRUNC typed_value TO type",
//t    "instruction : FSUB optional_fast type value ',' value",
//t    "instruction : GETELEMENTPTR type ',' typed_value ',' element_indices",
//t    "instruction : GETELEMENTPTR INBOUNDS type ',' typed_value ',' element_indices",
//t    "instruction : GETELEMENTPTR INBOUNDS NUW type ',' typed_value ',' element_indices",
//t    "instruction : GETELEMENTPTR NUW type ',' typed_value ',' element_indices",
//t    "instruction : ICMP icmp_condition type value ',' value",
//t    "instruction : INSERTELEMENT typed_value ',' typed_value ',' typed_value",
//t    "instruction : INSERTVALUE typed_value ',' typed_value ',' indices",
//t    "instruction : INTTOPTR typed_value TO type",
//t    "instruction : LANDINGPAD type CLEANUP",
//t    "instruction : LANDINGPAD type CATCH typed_value",
//t    "instruction : LOAD type ',' typed_pointer_value ',' ALIGN INTEGER",
//t    "instruction : LOAD ATOMIC type ',' typed_pointer_value ',' ALIGN INTEGER",
//t    "instruction : LOAD VOLATILE type ',' typed_pointer_value ',' ALIGN INTEGER",
//t    "instruction : LOAD ATOMIC VOLATILE type ',' typed_pointer_value ',' ALIGN INTEGER",
//t    "instruction : LOAD ATOMIC VOLATILE type ',' typed_pointer_value MONOTONIC ',' ALIGN INTEGER",
//t    "instruction : LSHR type value ',' value",
//t    "instruction : LSHR EXACT type value ',' value",
//t    "instruction : OR type value ',' value",
//t    "instruction : MUL type value ',' value",
//t    "instruction : MUL wrappings type value ',' value",
//t    "instruction : PHI type phi_vals",
//t    "instruction : PTRTOINT typed_value TO type",
//t    "instruction : SDIV type value ',' value",
//t    "instruction : SDIV EXACT type value ',' value",
//t    "instruction : SELECT optional_fast type value ',' typed_value ',' typed_value",
//t    "instruction : SEXT typed_value TO type",
//t    "instruction : SHL type value ',' value",
//t    "instruction : SHL wrappings type value ',' value",
//t    "instruction : SHUFFLEVECTOR typed_value ',' typed_value ',' typed_value",
//t    "instruction : SITOFP typed_value TO type",
//t    "instruction : SREM type value ',' value",
//t    "instruction : STORE typed_value ',' typed_pointer_value ',' ALIGN INTEGER",
//t    "instruction : STORE VOLATILE typed_value ',' typed_pointer_value ',' ALIGN INTEGER",
//t    "instruction : SUB type value ',' value",
//t    "instruction : SUB wrappings type value ',' value",
//t    "instruction : ATOMICRMW SUB type value ',' type value SEQ_CST ',' ALIGN INTEGER",
//t    "instruction : TRUNC typed_value TO type",
//t    "instruction : UDIV type value ',' value",
//t    "instruction : UITOFP typed_value TO type",
//t    "instruction : UREM type value ',' value",
//t    "instruction : XOR type value ',' value",
//t    "instruction : ZEXT typed_value TO type",
//t  };
//t public static string getRule (int index) {
//t    return yyRule [index];
//t }
//t}
  protected static readonly string [] yyNames = {    
    "end-of-file",null,null,null,null,null,null,null,null,null,null,null,
    null,null,null,null,null,null,null,null,null,null,null,null,null,null,
    null,null,null,null,null,null,null,"'!'",null,null,null,null,null,
    null,"'('","')'","'*'",null,"','",null,null,null,null,null,null,null,
    null,null,null,null,null,null,"':'",null,"'<'","'='","'>'",null,null,
    null,null,null,null,null,null,null,null,null,null,null,null,null,null,
    null,null,null,null,null,null,null,null,null,null,null,null,"'['",
    null,"']'",null,null,null,null,null,null,null,null,null,null,null,
    null,null,null,null,null,null,null,null,null,null,null,null,null,null,
    null,null,null,null,"'{'","'|'","'}'",null,null,null,null,null,null,
    null,null,null,null,null,null,null,null,null,null,null,null,null,null,
    null,null,null,null,null,null,null,null,null,null,null,null,null,null,
    null,null,null,null,null,null,null,null,null,null,null,null,null,null,
    null,null,null,null,null,null,null,null,null,null,null,null,null,null,
    null,null,null,null,null,null,null,null,null,null,null,null,null,null,
    null,null,null,null,null,null,null,null,null,null,null,null,null,null,
    null,null,null,null,null,null,null,null,null,null,null,null,null,null,
    null,null,null,null,null,null,null,null,null,null,null,null,null,null,
    null,null,null,null,null,null,null,null,null,null,null,null,null,
    "INTEGER","HEX_INTEGER","FLOAT_LITERAL","STRING","TRUE","FALSE",
    "UNDEF","VOID","NULL","LABEL","X","SOURCE_FILENAME","TARGET",
    "DATALAYOUT","TRIPLE","GLOBAL_SYMBOL","LOCAL_SYMBOL","META_SYMBOL",
    "META_SYMBOL_DEF","SYMBOL","DISTINCT","METADATA","CONSTANT_BYTES",
    "SECTION","TYPE","HALF","FLOAT","DOUBLE","X86_FP80","INTEGER_TYPE",
    "ZEROINITIALIZER","OPAQUE","PTR","DEFINE","DECLARE","UNNAMED_ADDR",
    "LOCAL_UNNAMED_ADDR","NOALIAS","ELLIPSIS","GLOBAL","CONSTANT",
    "PRIVATE","INTERNAL","EXTERNAL","LINKONCE","LINKONCE_ODR","WEAK",
    "WEAK_ODR","APPENDING","COMMON","FASTCC","SIGNEXT","ZEROEXT",
    "VOLATILE","RETURNED","DEREFERENCEABLE","AVAILABLE_EXTERNALLY",
    "PERSONALITY","SRET","CLEANUP","EXTERNALLY_INITIALIZED","NONNULL",
    "NOCAPTURE","WRITEONLY","READONLY","READNONE","HIDDEN","BYVAL",
    "ATTRIBUTE_GROUP_REF","ATTRIBUTES","NORECURSE","NOUNWIND","UNWIND",
    "SPECULATABLE","SSP","UWTABLE","ARGMEMONLY","SEQ_CST","DSO_LOCAL",
    "DSO_PREEMPTABLE","RET","BR","SWITCH","INDIRECTBR","INVOKE","RESUME",
    "CATCHSWITCH","CATCHRET","CLEANUPRET","UNREACHABLE","FNEG","ADD",
    "NUW","NSW","FADD","SUB","FSUB","MUL","FMUL","UDIV","SDIV","FDIV",
    "UREM","SREM","FREM","SHL","LSHR","EXACT","ASHR","AND","OR","XOR",
    "EXTRACTELEMENT","INSERTELEMENT","SHUFFLEVECTOR","EXTRACTVALUE",
    "INSERTVALUE","ALLOCA","LOAD","STORE","FENCE","CMPXCHG","ATOMICRMW",
    "GETELEMENTPTR","ALIGN","INBOUNDS","INRANGE","ADDRSPACE","TRUNC",
    "ZEXT","SEXT","FPTRUNC","FPEXT","TO","FPTOUI","FPTOSI","UITOFP",
    "SITOFP","PTRTOINT","INTTOPTR","BITCAST","ADDRSPACECAST","ICMP","EQ",
    "NE","UGT","UGE","ULT","ULE","SGT","SGE","SLT","SLE","FCMP","OEQ",
    "OGT","OGE","OLT","OLE","ONE","ORD","UEQ","UNE","UNO","FAST","PHI",
    "SELECT","CALL","TAIL","VA_ARG","ASM","SIDEEFFECT","LANDINGPAD",
    "CATCH","CATCHPAD","CLEANUPPAD","NOUNDEF","IMMARG","CAPTURES",
    "SWIFTCC","SWIFTTAILCC","SWIFTSELF","SWIFTERROR","SWIFTASYNC",
    "ATOMIC","MONOTONIC",
  };

  /** index-checked interface to yyNames[].
      @param token single character or %token value.
      @return token name or [illegal] or [unknown].
    */
  public static string yyname (int token) {
    if ((token < 0) || (token > yyNames.Length)) return "[illegal]";
    string name;
    if ((name = yyNames[token]) != null) return name;
    return "[unknown]";
  }

  //int yyExpectingState;
  /** computes list of expected tokens on error by tracing the tables.
      @param state for which to compute the list.
      @return list of token names.
    */
  protected int [] yyExpectingTokens (int state){
    int token, n, len = 0;
    bool[] ok = new bool[yyNames.Length];
    if ((n = yySindex[state]) != 0)
      for (token = n < 0 ? -n : 0;
           (token < yyNames.Length) && (n+token < yyTable.Length); ++ token)
        if (yyCheck[n+token] == token && !ok[token] && yyNames[token] != null) {
          ++ len;
          ok[token] = true;
        }
    if ((n = yyRindex[state]) != 0)
      for (token = n < 0 ? -n : 0;
           (token < yyNames.Length) && (n+token < yyTable.Length); ++ token)
        if (yyCheck[n+token] == token && !ok[token] && yyNames[token] != null) {
          ++ len;
          ok[token] = true;
        }
    int [] result = new int [len];
    for (n = token = 0; n < len;  ++ token)
      if (ok[token]) result[n++] = token;
    return result;
  }
  protected string[] yyExpecting (int state) {
    int [] tokens = yyExpectingTokens (state);
    string [] result = new string[tokens.Length];
    for (int n = 0; n < tokens.Length;  n++)
      result[n] = yyNames[tokens [n]];
    return result;
  }

  /** the generated parser, with debugging messages.
      Maintains a state and a value stack, currently with fixed maximum size.
      @param yyLex scanner.
      @param yydebug debug message writer implementing yyDebug, or null.
      @return result of the last reduction, if any.
      @throws yyException on irrecoverable parse error.
    */
  internal Object yyparse (yyParser.yyInput yyLex, Object yyd)
				 {
//t    this.debug = (yydebug.yyDebug)yyd;
    return yyparse(yyLex);
  }

  /** initial size and increment of the state/value stack [default 256].
      This is not final so that it can be overwritten outside of invocations
      of yyparse().
    */
  protected int yyMax;

  /** executed at the beginning of a reduce action.
      Used as $$ = yyDefault($1), prior to the user-specified action, if any.
      Can be overwritten to provide deep copy, etc.
      @param first value for $1, or null.
      @return first.
    */
  protected Object yyDefault (Object first) {
    return first;
  }

	static int[] global_yyStates;
	static object[] global_yyVals;
	protected bool use_global_stacks;
	object[] yyVals;					// value stack
	object yyVal;						// value stack ptr
	int yyToken;						// current input
	int yyTop;

  /** the generated parser.
      Maintains a state and a value stack, currently with fixed maximum size.
      @param yyLex scanner.
      @return result of the last reduction, if any.
      @throws yyException on irrecoverable parse error.
    */
  internal Object yyparse (yyParser.yyInput yyLex)
  {
    if (yyMax <= 0) yyMax = 256;		// initial size
    int yyState = 0;                   // state stack ptr
    int [] yyStates;               	// state stack 
    yyVal = null;
    yyToken = -1;
    int yyErrorFlag = 0;				// #tks to shift
	if (use_global_stacks && global_yyStates != null) {
		yyVals = global_yyVals;
		yyStates = global_yyStates;
   } else {
		yyVals = new object [yyMax];
		yyStates = new int [yyMax];
		if (use_global_stacks) {
			global_yyVals = yyVals;
			global_yyStates = yyStates;
		}
	}

    /*yyLoop:*/ for (yyTop = 0;; ++ yyTop) {
      if (yyTop >= yyStates.Length) {			// dynamically increase
        global::System.Array.Resize (ref yyStates, yyStates.Length+yyMax);
        global::System.Array.Resize (ref yyVals, yyVals.Length+yyMax);
      }
      yyStates[yyTop] = yyState;
      yyVals[yyTop] = yyVal;
//t      if (debug != null) debug.push(yyState, yyVal);

      /*yyDiscarded:*/ while (true) {	// discarding a token does not change stack
        int yyN;
        if ((yyN = yyDefRed[yyState]) == 0) {	// else [default] reduce (yyN)
          if (yyToken < 0) {
            yyToken = yyLex.advance() ? yyLex.token() : 0;
//t            if (debug != null)
//t              debug.lex(yyState, yyToken, yyname(yyToken), yyLex.value());
          }
          if ((yyN = yySindex[yyState]) != 0 && ((yyN += yyToken) >= 0)
              && (yyN < yyTable.Length) && (yyCheck[yyN] == yyToken)) {
//t            if (debug != null)
//t              debug.shift(yyState, yyTable[yyN], yyErrorFlag-1);
            yyState = yyTable[yyN];		// shift to yyN
            yyVal = yyLex.value();
            yyToken = -1;
            if (yyErrorFlag > 0) -- yyErrorFlag;
            goto continue_yyLoop;
          }
          if ((yyN = yyRindex[yyState]) != 0 && (yyN += yyToken) >= 0
              && yyN < yyTable.Length && yyCheck[yyN] == yyToken)
            yyN = yyTable[yyN];			// reduce (yyN)
          else
            switch (yyErrorFlag) {
  
            case 0:
              //yyExpectingState = yyState;
              SyntaxError(yyname (yyToken), String.Join(", ", yyExpecting(yyState)));
//t              if (debug != null) debug.error("syntax error");
              if (yyToken == 0 /*eof*/ || yyToken == eof_token) throw new yyParser.yyUnexpectedEof ();
              goto case 1;
            case 1: case 2:
              yyErrorFlag = 3;
              do {
                if ((yyN = yySindex[yyStates[yyTop]]) != 0
                    && (yyN += Token.yyErrorCode) >= 0 && yyN < yyTable.Length
                    && yyCheck[yyN] == Token.yyErrorCode) {
//t                  if (debug != null)
//t                    debug.shift(yyStates[yyTop], yyTable[yyN], 3);
                  yyState = yyTable[yyN];
                  yyVal = yyLex.value();
                  goto continue_yyLoop;
                }
//t                if (debug != null) debug.pop(yyStates[yyTop]);
              } while (-- yyTop >= 0);
//t              if (debug != null) debug.reject();
              throw new yyParser.yyException("Irrecoverable syntax error");
  
            case 3:
              if (yyToken == 0) {
//t                if (debug != null) debug.reject();
                throw new yyParser.yyException("Irrecoverable syntax error at end-of-file");
              }
//t              if (debug != null)
//t                debug.discard(yyState, yyToken, yyname(yyToken),
//t  							yyLex.value());
              yyToken = -1;
              goto continue_yyDiscarded;		// leave stack alone
            }
        }
        int yyV = yyTop + 1-yyLen[yyN];
//t        if (debug != null)
//t          debug.reduce(yyState, yyStates[yyV-1], yyN, YYRules.getRule (yyN), yyLen[yyN]);
        yyVal = yyV > yyTop ? null : yyVals[yyV]; // yyVal = yyDefault(yyV > yyTop ? null : yyVals[yyV]);
        switch (yyN) {
case 4:
#line 65 "Iril/IR/IR.jay"
  {
        module.SourceFilename = (string)yyVals[0+yyTop];
    }
  break;
case 5:
#line 69 "Iril/IR/IR.jay"
  {
        module.TargetDatalayout = (string)yyVals[0+yyTop];
    }
  break;
case 6:
#line 73 "Iril/IR/IR.jay"
  {
        module.TargetTriple = (string)yyVals[0+yyTop];
    }
  break;
case 7:
#line 77 "Iril/IR/IR.jay"
  {
        module.IdentifiedStructures[(Symbol)yyVals[-3+yyTop]] = (StructureType)yyVals[0+yyTop];
    }
  break;
case 8:
#line 81 "Iril/IR/IR.jay"
  {
        module.IdentifiedStructures[(Symbol)yyVals[-3+yyTop]] = OpaqueStructureType.Opaque;
    }
  break;
case 9:
  case_9();
  break;
case 10:
  case_10();
  break;
case 11:
  case_11();
  break;
case 14:
#line 102 "Iril/IR/IR.jay"
  {
        module.Metadata[(Symbol)yyVals[-4+yyTop]] = new List<object> (0);
    }
  break;
case 15:
#line 106 "Iril/IR/IR.jay"
  {
        module.Metadata[(Symbol)yyVals[-5+yyTop]] = yyVals[-1+yyTop];
    }
  break;
case 16:
  case_16();
  break;
case 17:
#line 115 "Iril/IR/IR.jay"
  {
        module.Metadata[(Symbol)yyVals[-6+yyTop]] = yyVals[-1+yyTop];
    }
  break;
case 18:
  case_18();
  break;
case 19:
#line 127 "Iril/IR/IR.jay"
  {
        yyVal = new GlobalVariable ((GlobalSymbol)yyVals[-5+yyTop], (LType)yyVals[-1+yyTop], (Value)yyVals[0+yyTop], isPrivate: false, isExternal: false, isConstant: (bool)yyVals[-2+yyTop]);
    }
  break;
case 20:
#line 132 "Iril/IR/IR.jay"
  {
        yyVal = new GlobalVariable ((GlobalSymbol)yyVals[-4+yyTop], (LType)yyVals[0+yyTop], null, isPrivate: false, isExternal: true, isConstant: (bool)yyVals[-1+yyTop]);
    }
  break;
case 21:
#line 136 "Iril/IR/IR.jay"
  {
        yyVal = new GlobalVariable ((GlobalSymbol)yyVals[-7+yyTop], (LType)yyVals[-3+yyTop], null, isPrivate: false, isExternal: true, isConstant: (bool)yyVals[-4+yyTop]);
    }
  break;
case 22:
#line 140 "Iril/IR/IR.jay"
  {
        yyVal = new GlobalVariable ((GlobalSymbol)yyVals[-5+yyTop], (LType)yyVals[0+yyTop], null, isPrivate: false, isExternal: true, isConstant: (bool)yyVals[-1+yyTop]);
    }
  break;
case 23:
#line 144 "Iril/IR/IR.jay"
  {
        yyVal = new GlobalVariable ((GlobalSymbol)yyVals[-8+yyTop], (LType)yyVals[-3+yyTop], null, isPrivate: false, isExternal: true, isConstant: (bool)yyVals[-4+yyTop]);
    }
  break;
case 24:
#line 148 "Iril/IR/IR.jay"
  {
        yyVal = new GlobalVariable ((GlobalSymbol)yyVals[-5+yyTop], (LType)yyVals[0+yyTop], null, isPrivate: false, isExternal: true, isConstant: (bool)yyVals[-1+yyTop]);
    }
  break;
case 25:
#line 152 "Iril/IR/IR.jay"
  {
        yyVal = new GlobalVariable ((GlobalSymbol)yyVals[-5+yyTop], (LType)yyVals[0+yyTop], null, isPrivate: false, isExternal: (bool)yyVals[-3+yyTop], isConstant: (bool)yyVals[-3+yyTop]);
    }
  break;
case 26:
#line 156 "Iril/IR/IR.jay"
  {
        yyVal = new GlobalVariable ((GlobalSymbol)yyVals[-8+yyTop], (LType)yyVals[-4+yyTop], (Value)yyVals[-3+yyTop], isPrivate: false, isExternal: false, isConstant: (bool)yyVals[-5+yyTop]);
    }
  break;
case 27:
#line 160 "Iril/IR/IR.jay"
  {
        yyVal = new GlobalVariable ((GlobalSymbol)yyVals[-9+yyTop], (LType)yyVals[-4+yyTop], (Value)yyVals[-3+yyTop], isPrivate: false, isExternal: false, isConstant: (bool)yyVals[-5+yyTop]);
    }
  break;
case 28:
#line 164 "Iril/IR/IR.jay"
  {
        yyVal = new GlobalVariable ((GlobalSymbol)yyVals[-12+yyTop], (LType)yyVals[-7+yyTop], (Value)yyVals[-6+yyTop], isPrivate: false, isExternal: false, isConstant: (bool)yyVals[-8+yyTop]);
    }
  break;
case 29:
#line 168 "Iril/IR/IR.jay"
  {
        yyVal = new GlobalVariable ((GlobalSymbol)yyVals[-8+yyTop], (LType)yyVals[-5+yyTop], (Value)yyVals[-4+yyTop], isPrivate: false, isExternal: false, isConstant: (bool)yyVals[-6+yyTop]);
    }
  break;
case 30:
#line 172 "Iril/IR/IR.jay"
  {
        yyVal = new GlobalVariable ((GlobalSymbol)yyVals[-9+yyTop], (LType)yyVals[-5+yyTop], (Value)yyVals[-4+yyTop], isPrivate: false, isExternal: false, isConstant: (bool)yyVals[-6+yyTop]);
    }
  break;
case 31:
#line 176 "Iril/IR/IR.jay"
  {
        yyVal = new GlobalVariable ((GlobalSymbol)yyVals[-9+yyTop], (LType)yyVals[-4+yyTop], (Value)yyVals[-3+yyTop], isPrivate: false, isExternal: false, isConstant: (bool)yyVals[-5+yyTop]);
    }
  break;
case 32:
#line 180 "Iril/IR/IR.jay"
  {
        yyVal = new GlobalVariable ((GlobalSymbol)yyVals[-9+yyTop], (LType)yyVals[-5+yyTop], (Value)yyVals[-4+yyTop], isPrivate: false, isExternal: false, isConstant: (bool)yyVals[-6+yyTop]);
    }
  break;
case 33:
#line 184 "Iril/IR/IR.jay"
  {
        yyVal = new GlobalVariable ((GlobalSymbol)yyVals[-9+yyTop], (LType)yyVals[-4+yyTop], (Value)yyVals[-3+yyTop], isPrivate: (bool)yyVals[-7+yyTop], isExternal: false, isConstant: (bool)yyVals[-5+yyTop]);
    }
  break;
case 34:
#line 188 "Iril/IR/IR.jay"
  {
        yyVal = new GlobalVariable ((GlobalSymbol)yyVals[-6+yyTop], (LType)yyVals[-1+yyTop], (Value)yyVals[0+yyTop], isPrivate: (bool)yyVals[-4+yyTop], isExternal: false, isConstant: (bool)yyVals[-2+yyTop]);
    }
  break;
case 35:
#line 192 "Iril/IR/IR.jay"
  {
        yyVal = new GlobalVariable ((GlobalSymbol)yyVals[-10+yyTop], (LType)yyVals[-5+yyTop], (Value)yyVals[-4+yyTop], isPrivate: (bool)yyVals[-8+yyTop], isExternal: false, isConstant: (bool)yyVals[-6+yyTop]);
    }
  break;
case 36:
#line 197 "Iril/IR/IR.jay"
  {
        yyVal = new GlobalVariable ((GlobalSymbol)yyVals[-5+yyTop], (LType)yyVals[-1+yyTop], (Value)yyVals[0+yyTop], isPrivate: (bool)yyVals[-3+yyTop], isExternal: false, isConstant: (bool)yyVals[-2+yyTop]);
    }
  break;
case 37:
#line 201 "Iril/IR/IR.jay"
  {
        yyVal = new GlobalVariable ((GlobalSymbol)yyVals[-8+yyTop], (LType)yyVals[-4+yyTop], (Value)yyVals[-3+yyTop], isPrivate: (bool)yyVals[-6+yyTop], isExternal: false, isConstant: (bool)yyVals[-5+yyTop]);
    }
  break;
case 38:
#line 205 "Iril/IR/IR.jay"
  {
        yyVal = new GlobalVariable ((GlobalSymbol)yyVals[-8+yyTop], (LType)yyVals[-4+yyTop], (Value)yyVals[-3+yyTop], isPrivate: (bool)yyVals[-6+yyTop], isExternal: false, isConstant: (bool)yyVals[-5+yyTop]);
    }
  break;
case 39:
#line 209 "Iril/IR/IR.jay"
  {
        yyVal = new GlobalVariable ((GlobalSymbol)yyVals[-11+yyTop], (LType)yyVals[-7+yyTop], (Value)yyVals[-6+yyTop], isPrivate: (bool)yyVals[-9+yyTop], isExternal: false, isConstant: (bool)yyVals[-8+yyTop]);
    }
  break;
case 40:
#line 213 "Iril/IR/IR.jay"
  {
        yyVal = new GlobalVariable ((GlobalSymbol)yyVals[-8+yyTop], (LType)yyVals[-3+yyTop], null, isPrivate: false, isExternal: (bool)yyVals[-6+yyTop], isConstant: (bool)yyVals[-4+yyTop]);
    }
  break;
case 41:
#line 217 "Iril/IR/IR.jay"
  {
        yyVal = new GlobalVariable ((GlobalSymbol)yyVals[-9+yyTop], (LType)yyVals[-4+yyTop], (Value)yyVals[-3+yyTop], isPrivate: false, isExternal: (bool)yyVals[-7+yyTop], isConstant: (bool)yyVals[-5+yyTop]);
    }
  break;
case 42:
#line 221 "Iril/IR/IR.jay"
  {
        yyVal = new GlobalVariable ((GlobalSymbol)yyVals[-10+yyTop], (LType)yyVals[-5+yyTop], (Value)yyVals[-4+yyTop], isPrivate: false, isExternal: (bool)yyVals[-8+yyTop], isConstant: (bool)yyVals[-6+yyTop]);
    }
  break;
case 43:
#line 225 "Iril/IR/IR.jay"
  {
        yyVal = new GlobalVariable ((GlobalSymbol)yyVals[-8+yyTop], (LType)yyVals[-4+yyTop], (Value)yyVals[-3+yyTop], isPrivate: false, isExternal: (bool)yyVals[-6+yyTop], isConstant: (bool)yyVals[-5+yyTop]);
    }
  break;
case 44:
#line 229 "Iril/IR/IR.jay"
  {
        yyVal = new GlobalVariable ((GlobalSymbol)yyVals[-9+yyTop], (LType)yyVals[-5+yyTop], (Value)yyVals[-4+yyTop], isPrivate: false, isExternal: (bool)yyVals[-7+yyTop], isConstant: (bool)yyVals[-6+yyTop]);
    }
  break;
case 45:
#line 233 "Iril/IR/IR.jay"
  {
        yyVal = new GlobalVariable ((GlobalSymbol)yyVals[-7+yyTop], (LType)yyVals[-3+yyTop], null, isPrivate: false, isExternal: (bool)yyVals[-5+yyTop], isConstant: (bool)yyVals[-4+yyTop]);
    }
  break;
case 46:
#line 237 "Iril/IR/IR.jay"
  {
        yyVal = new GlobalVariable ((GlobalSymbol)yyVals[-4+yyTop], (LType)yyVals[0+yyTop], null, isPrivate: false, isExternal: (bool)yyVals[-2+yyTop], isConstant: (bool)yyVals[-1+yyTop]);
    }
  break;
case 47:
#line 241 "Iril/IR/IR.jay"
  { yyVal = false; }
  break;
case 48:
#line 242 "Iril/IR/IR.jay"
  { yyVal = true; }
  break;
case 49:
#line 250 "Iril/IR/IR.jay"
  { yyVal = true; }
  break;
case 50:
#line 254 "Iril/IR/IR.jay"
  { yyVal = true; }
  break;
case 51:
#line 255 "Iril/IR/IR.jay"
  { yyVal = false; }
  break;
case 52:
#line 256 "Iril/IR/IR.jay"
  { yyVal = true; }
  break;
case 53:
#line 257 "Iril/IR/IR.jay"
  { yyVal = true; }
  break;
case 54:
#line 258 "Iril/IR/IR.jay"
  { yyVal = true; }
  break;
case 55:
#line 259 "Iril/IR/IR.jay"
  { yyVal = true; }
  break;
case 56:
#line 260 "Iril/IR/IR.jay"
  { yyVal = true; }
  break;
case 57:
#line 261 "Iril/IR/IR.jay"
  { yyVal = false; }
  break;
case 58:
#line 265 "Iril/IR/IR.jay"
  { yyVal = true; }
  break;
case 59:
#line 269 "Iril/IR/IR.jay"
  { yyVal = true; }
  break;
case 60:
  case_60();
  break;
case 61:
  case_61();
  break;
case 62:
#line 286 "Iril/IR/IR.jay"
  { yyVal = Tuple.Create (yyVals[-2+yyTop], yyVals[0+yyTop]); }
  break;
case 63:
#line 287 "Iril/IR/IR.jay"
  { yyVal = Tuple.Create (yyVals[-2+yyTop], yyVals[0+yyTop]); }
  break;
case 64:
#line 288 "Iril/IR/IR.jay"
  { yyVal = Tuple.Create (yyVals[-2+yyTop], yyVals[0+yyTop]); }
  break;
case 65:
#line 292 "Iril/IR/IR.jay"
  {
        yyVal = Tuple.Create (yyVals[-5+yyTop], yyVals[-3+yyTop]);
    }
  break;
case 66:
#line 296 "Iril/IR/IR.jay"
  {
        yyVal = Tuple.Create (yyVals[-4+yyTop], yyVals[-2+yyTop]);
    }
  break;
case 77:
#line 325 "Iril/IR/IR.jay"
  {
        yyVal = NewSyms (yyVals[-1+yyTop], (MetaSymbol)yyVals[0+yyTop]);
    }
  break;
case 78:
#line 329 "Iril/IR/IR.jay"
  {
        yyVal = SymsAdd (yyVals[-2+yyTop], yyVals[-1+yyTop], (MetaSymbol)yyVals[0+yyTop]);
    }
  break;
case 79:
#line 336 "Iril/IR/IR.jay"
  {
        yyVal = NewList (yyVals[0+yyTop]);
    }
  break;
case 80:
#line 340 "Iril/IR/IR.jay"
  {
        yyVal = ListAdd (yyVals[-1+yyTop], yyVals[0+yyTop]);
    }
  break;
case 81:
#line 344 "Iril/IR/IR.jay"
  {
        yyVal = ListAdd (yyVals[-2+yyTop], yyVals[0+yyTop]);
    }
  break;
case 82:
#line 348 "Iril/IR/IR.jay"
  {
        yyVal = ListAdd (yyVals[-2+yyTop], yyVals[0+yyTop]);
    }
  break;
case 83:
#line 352 "Iril/IR/IR.jay"
  {
        yyVal = ListAdd (yyVals[-2+yyTop], yyVals[0+yyTop]);
    }
  break;
case 102:
#line 386 "Iril/IR/IR.jay"
  {
        yyVal = LiteralStructureType.Empty;
    }
  break;
case 103:
#line 390 "Iril/IR/IR.jay"
  {
        yyVal = new LiteralStructureType (false, (List<LType>)yyVals[-1+yyTop]);
    }
  break;
case 104:
#line 394 "Iril/IR/IR.jay"
  {
        yyVal = new PackedStructureType ((List<LType>)yyVals[-2+yyTop]);
    }
  break;
case 105:
#line 401 "Iril/IR/IR.jay"
  {
        yyVal = NewList ((LType)yyVals[0+yyTop]);
    }
  break;
case 106:
#line 405 "Iril/IR/IR.jay"
  {
        yyVal = ListAdd (yyVals[-2+yyTop], (LType)yyVals[0+yyTop]);
    }
  break;
case 108:
#line 410 "Iril/IR/IR.jay"
  { yyVal = VoidType.Void; }
  break;
case 111:
#line 416 "Iril/IR/IR.jay"
  { yyVal = FloatType.Half; }
  break;
case 112:
#line 417 "Iril/IR/IR.jay"
  { yyVal = FloatType.Float; }
  break;
case 113:
#line 418 "Iril/IR/IR.jay"
  { yyVal = FloatType.Double; }
  break;
case 114:
#line 419 "Iril/IR/IR.jay"
  { yyVal = FloatType.X86_FP80; }
  break;
case 115:
#line 423 "Iril/IR/IR.jay"
  {
        yyVal = new FunctionType ((LType)yyVals[-2+yyTop], Enumerable.Empty<LType>());
    }
  break;
case 116:
#line 427 "Iril/IR/IR.jay"
  {
        yyVal = new FunctionType ((LType)yyVals[-3+yyTop], (List<LType>)yyVals[-1+yyTop]);
    }
  break;
case 117:
#line 431 "Iril/IR/IR.jay"
  {
        yyVal = new PointerType ((LType)yyVals[-2+yyTop], 0);
    }
  break;
case 118:
#line 435 "Iril/IR/IR.jay"
  {
        yyVal = new PointerType ((LType)yyVals[-4+yyTop], 0);
    }
  break;
case 119:
#line 439 "Iril/IR/IR.jay"
  {
        yyVal = new NamedType ((Symbol)yyVals[0+yyTop]);
    }
  break;
case 120:
#line 443 "Iril/IR/IR.jay"
  {
        yyVal = PointerType.OpaquePointer;
    }
  break;
case 121:
#line 447 "Iril/IR/IR.jay"
  {
        yyVal = new VectorType ((int)(BigInteger)yyVals[-3+yyTop], (LType)yyVals[-1+yyTop]);
    }
  break;
case 122:
#line 451 "Iril/IR/IR.jay"
  {
        yyVal = new ArrayType ((long)(BigInteger)yyVals[-3+yyTop], (LType)yyVals[-1+yyTop]);
    }
  break;
case 126:
#line 467 "Iril/IR/IR.jay"
  {
        yyVal = NewList ((LType)yyVals[0+yyTop]);
    }
  break;
case 127:
#line 471 "Iril/IR/IR.jay"
  {
        yyVal = ListAdd (yyVals[-2+yyTop], (LType)yyVals[0+yyTop]);
    }
  break;
case 129:
#line 479 "Iril/IR/IR.jay"
  {
        yyVal = VarArgsType.VarArgs;
    }
  break;
case 130:
  case_130();
  break;
case 131:
  case_131();
  break;
case 143:
#line 513 "Iril/IR/IR.jay"
  {
        yyVal = Tuple.Create ((object)true, yyVals[0+yyTop]);
    }
  break;
case 144:
#line 517 "Iril/IR/IR.jay"
  {
        yyVal = Tuple.Create ((object)true, yyVals[0+yyTop]);
    }
  break;
case 145:
#line 521 "Iril/IR/IR.jay"
  {
        yyVal = Tuple.Create (yyVals[-1+yyTop], yyVals[0+yyTop]);
    }
  break;
case 146:
#line 525 "Iril/IR/IR.jay"
  {
        yyVal = Tuple.Create (yyVals[-2+yyTop], yyVals[0+yyTop]);
    }
  break;
case 147:
#line 529 "Iril/IR/IR.jay"
  {
        yyVal = Tuple.Create (yyVals[-3+yyTop], yyVals[0+yyTop]);
    }
  break;
case 148:
#line 533 "Iril/IR/IR.jay"
  {
        yyVal = Tuple.Create (yyVals[-2+yyTop], yyVals[0+yyTop]);
    }
  break;
case 149:
#line 540 "Iril/IR/IR.jay"
  {
        yyVal = true;
    }
  break;
case 150:
#line 544 "Iril/IR/IR.jay"
  {
        yyVal = true;
    }
  break;
case 151:
#line 548 "Iril/IR/IR.jay"
  {
        yyVal = true;
    }
  break;
case 156:
#line 559 "Iril/IR/IR.jay"
  {
        yyVal = yyVals[0+yyTop];
    }
  break;
case 159:
#line 571 "Iril/IR/IR.jay"
  {
        yyVal = new FunctionDeclaration ((LType)yyVals[-2+yyTop], (GlobalSymbol)yyVals[-1+yyTop], (IEnumerable<Parameter>)yyVals[0+yyTop]);
    }
  break;
case 160:
#line 575 "Iril/IR/IR.jay"
  {
        yyVal = new FunctionDeclaration ((LType)yyVals[-3+yyTop], (GlobalSymbol)yyVals[-2+yyTop], (IEnumerable<Parameter>)yyVals[-1+yyTop]);
    }
  break;
case 161:
#line 579 "Iril/IR/IR.jay"
  {
        yyVal = new FunctionDeclaration ((LType)yyVals[-3+yyTop], (GlobalSymbol)yyVals[-2+yyTop], (IEnumerable<Parameter>)yyVals[-1+yyTop]);
    }
  break;
case 162:
#line 583 "Iril/IR/IR.jay"
  {
        yyVal = new FunctionDeclaration ((LType)yyVals[-3+yyTop], (GlobalSymbol)yyVals[-2+yyTop], (IEnumerable<Parameter>)yyVals[-1+yyTop]);
    }
  break;
case 163:
#line 587 "Iril/IR/IR.jay"
  {
        yyVal = new FunctionDeclaration ((LType)yyVals[-3+yyTop], (GlobalSymbol)yyVals[-2+yyTop], (IEnumerable<Parameter>)yyVals[-1+yyTop]);
    }
  break;
case 164:
#line 591 "Iril/IR/IR.jay"
  {
        yyVal = new FunctionDeclaration ((LType)yyVals[-2+yyTop], (GlobalSymbol)yyVals[-1+yyTop], (IEnumerable<Parameter>)yyVals[0+yyTop]);
    }
  break;
case 165:
#line 595 "Iril/IR/IR.jay"
  {
        yyVal = new FunctionDeclaration ((LType)yyVals[-3+yyTop], (GlobalSymbol)yyVals[-2+yyTop], (IEnumerable<Parameter>)yyVals[-1+yyTop]);
    }
  break;
case 166:
#line 599 "Iril/IR/IR.jay"
  {
        yyVal = new FunctionDeclaration ((LType)yyVals[-3+yyTop], (GlobalSymbol)yyVals[-2+yyTop], (IEnumerable<Parameter>)yyVals[-1+yyTop]);
    }
  break;
case 170:
#line 609 "Iril/IR/IR.jay"
  { yyVal = yyVals[-1+yyTop]; }
  break;
case 171:
#line 610 "Iril/IR/IR.jay"
  { yyVal = Enumerable.Empty<Parameter> (); }
  break;
case 172:
#line 617 "Iril/IR/IR.jay"
  {
        yyVal = NewList ((Parameter)yyVals[0+yyTop]);
    }
  break;
case 173:
#line 621 "Iril/IR/IR.jay"
  {
        yyVal = ListAdd (yyVals[-2+yyTop], (Parameter)yyVals[0+yyTop]);
    }
  break;
case 174:
#line 628 "Iril/IR/IR.jay"
  {
        yyVal = new Parameter (LocalSymbol.None, (LType)yyVals[0+yyTop]);
    }
  break;
case 175:
#line 632 "Iril/IR/IR.jay"
  {
        yyVal = new Parameter ((LocalSymbol)yyVals[0+yyTop], (LType)yyVals[-1+yyTop]);
    }
  break;
case 176:
#line 636 "Iril/IR/IR.jay"
  {
        yyVal = new Parameter (LocalSymbol.None, (LType)yyVals[-1+yyTop]);
    }
  break;
case 177:
#line 640 "Iril/IR/IR.jay"
  {
        yyVal = new Parameter ((LocalSymbol)yyVals[0+yyTop], (LType)yyVals[-2+yyTop]);
    }
  break;
case 178:
#line 644 "Iril/IR/IR.jay"
  {
        yyVal = new Parameter (LocalSymbol.None, IntegerType.I32);
    }
  break;
case 179:
#line 648 "Iril/IR/IR.jay"
  {
        yyVal = new Parameter (LocalSymbol.None, VarArgsType.VarArgs);
    }
  break;
case 181:
#line 656 "Iril/IR/IR.jay"
  {
        yyVal = ((ParameterAttributes)yyVals[-1+yyTop]) | ((ParameterAttributes)yyVals[0+yyTop]);
    }
  break;
case 182:
#line 660 "Iril/IR/IR.jay"
  { yyVal = ParameterAttributes.NonNull; }
  break;
case 183:
#line 661 "Iril/IR/IR.jay"
  { yyVal = ParameterAttributes.NoCapture; }
  break;
case 184:
#line 662 "Iril/IR/IR.jay"
  { yyVal = ParameterAttributes.NoCapture; }
  break;
case 185:
#line 663 "Iril/IR/IR.jay"
  { yyVal = ParameterAttributes.None; }
  break;
case 186:
#line 664 "Iril/IR/IR.jay"
  { yyVal = ParameterAttributes.None; }
  break;
case 187:
#line 665 "Iril/IR/IR.jay"
  { yyVal = ParameterAttributes.None; }
  break;
case 188:
#line 666 "Iril/IR/IR.jay"
  { yyVal = ParameterAttributes.NoUndef; }
  break;
case 189:
#line 667 "Iril/IR/IR.jay"
  { yyVal = ParameterAttributes.ImmediateArgument; }
  break;
case 190:
#line 668 "Iril/IR/IR.jay"
  { yyVal = ParameterAttributes.ReadOnly; }
  break;
case 191:
#line 669 "Iril/IR/IR.jay"
  { yyVal = ParameterAttributes.WriteOnly; }
  break;
case 192:
#line 670 "Iril/IR/IR.jay"
  { yyVal = ParameterAttributes.ReadNone; }
  break;
case 193:
#line 671 "Iril/IR/IR.jay"
  { yyVal = ParameterAttributes.SignExtend; }
  break;
case 194:
#line 672 "Iril/IR/IR.jay"
  { yyVal = ParameterAttributes.ZeroExtend; }
  break;
case 195:
#line 673 "Iril/IR/IR.jay"
  { yyVal = ParameterAttributes.Returned; }
  break;
case 196:
#line 674 "Iril/IR/IR.jay"
  { yyVal = ParameterAttributes.StructureReturn; }
  break;
case 197:
#line 675 "Iril/IR/IR.jay"
  { yyVal = ParameterAttributes.StructureReturn; }
  break;
case 198:
#line 676 "Iril/IR/IR.jay"
  { yyVal = ParameterAttributes.NoAlias; }
  break;
case 199:
#line 677 "Iril/IR/IR.jay"
  { yyVal = ParameterAttributes.Byval; }
  break;
case 200:
#line 678 "Iril/IR/IR.jay"
  { yyVal = ParameterAttributes.Byval; }
  break;
case 201:
#line 682 "Iril/IR/IR.jay"
  {
        yyVal = ParameterAttributes.Dereferenceable;
    }
  break;
case 202:
#line 686 "Iril/IR/IR.jay"
  {
        yyVal = ParameterAttributes.Align8;
    }
  break;
case 216:
#line 721 "Iril/IR/IR.jay"
  { yyVal = IcmpCondition.Equal; }
  break;
case 217:
#line 722 "Iril/IR/IR.jay"
  { yyVal = IcmpCondition.NotEqual; }
  break;
case 218:
#line 723 "Iril/IR/IR.jay"
  { yyVal = IcmpCondition.UnsignedGreaterThan; }
  break;
case 219:
#line 724 "Iril/IR/IR.jay"
  { yyVal = IcmpCondition.UnsignedGreaterThanOrEqual; }
  break;
case 220:
#line 725 "Iril/IR/IR.jay"
  { yyVal = IcmpCondition.UnsignedLessThan; }
  break;
case 221:
#line 726 "Iril/IR/IR.jay"
  { yyVal = IcmpCondition.UnsignedLessThanOrEqual; }
  break;
case 222:
#line 727 "Iril/IR/IR.jay"
  { yyVal = IcmpCondition.SignedGreaterThan; }
  break;
case 223:
#line 728 "Iril/IR/IR.jay"
  { yyVal = IcmpCondition.SignedGreaterThanOrEqual; }
  break;
case 224:
#line 729 "Iril/IR/IR.jay"
  { yyVal = IcmpCondition.SignedLessThan; }
  break;
case 225:
#line 730 "Iril/IR/IR.jay"
  { yyVal = IcmpCondition.SignedLessThanOrEqual; }
  break;
case 226:
#line 734 "Iril/IR/IR.jay"
  { yyVal = FcmpCondition.True; }
  break;
case 227:
#line 735 "Iril/IR/IR.jay"
  { yyVal = FcmpCondition.False; }
  break;
case 228:
#line 736 "Iril/IR/IR.jay"
  { yyVal = FcmpCondition.Ordered; }
  break;
case 229:
#line 737 "Iril/IR/IR.jay"
  { yyVal = FcmpCondition.OrderedEqual; }
  break;
case 230:
#line 738 "Iril/IR/IR.jay"
  { yyVal = FcmpCondition.OrderedNotEqual; }
  break;
case 231:
#line 739 "Iril/IR/IR.jay"
  { yyVal = FcmpCondition.OrderedGreaterThan; }
  break;
case 232:
#line 740 "Iril/IR/IR.jay"
  { yyVal = FcmpCondition.OrderedGreaterThanOrEqual; }
  break;
case 233:
#line 741 "Iril/IR/IR.jay"
  { yyVal = FcmpCondition.OrderedLessThan; }
  break;
case 234:
#line 742 "Iril/IR/IR.jay"
  { yyVal = FcmpCondition.OrderedLessThanOrEqual; }
  break;
case 235:
#line 743 "Iril/IR/IR.jay"
  { yyVal = FcmpCondition.Unordered; }
  break;
case 236:
#line 744 "Iril/IR/IR.jay"
  { yyVal = FcmpCondition.UnorderedEqual; }
  break;
case 237:
#line 745 "Iril/IR/IR.jay"
  { yyVal = FcmpCondition.UnorderedNotEqual; }
  break;
case 238:
#line 746 "Iril/IR/IR.jay"
  { yyVal = FcmpCondition.UnorderedGreaterThan; }
  break;
case 239:
#line 747 "Iril/IR/IR.jay"
  { yyVal = FcmpCondition.UnorderedGreaterThanOrEqual; }
  break;
case 240:
#line 748 "Iril/IR/IR.jay"
  { yyVal = FcmpCondition.UnorderedLessThan; }
  break;
case 241:
#line 749 "Iril/IR/IR.jay"
  { yyVal = FcmpCondition.UnorderedLessThanOrEqual; }
  break;
case 242:
#line 753 "Iril/IR/IR.jay"
  { yyVal = new GlobalValue ((GlobalSymbol)yyVals[0+yyTop]); }
  break;
case 246:
#line 763 "Iril/IR/IR.jay"
  { yyVal = new LocalValue ((LocalSymbol)yyVals[0+yyTop]); }
  break;
case 247:
#line 767 "Iril/IR/IR.jay"
  {
        yyVal = new IntToPointerValue ((TypedValue)yyVals[-3+yyTop], (LType)yyVals[-1+yyTop]);
    }
  break;
case 248:
#line 771 "Iril/IR/IR.jay"
  {
        yyVal = new GetElementPointerValue ((LType)yyVals[-5+yyTop], (TypedValue)yyVals[-3+yyTop], (List<TypedValue>)yyVals[-1+yyTop]);
    }
  break;
case 249:
#line 775 "Iril/IR/IR.jay"
  {
        yyVal = new GetElementPointerValue ((LType)yyVals[-5+yyTop], (TypedValue)yyVals[-3+yyTop], (List<TypedValue>)yyVals[-1+yyTop]);
    }
  break;
case 250:
#line 779 "Iril/IR/IR.jay"
  {
        yyVal = new GetElementPointerValue ((LType)yyVals[-5+yyTop], (TypedValue)yyVals[-3+yyTop], (List<TypedValue>)yyVals[-1+yyTop]);
    }
  break;
case 251:
#line 783 "Iril/IR/IR.jay"
  {
        yyVal = new BitcastValue ((TypedValue)yyVals[-3+yyTop], (LType)yyVals[-1+yyTop]);
    }
  break;
case 252:
#line 787 "Iril/IR/IR.jay"
  {
        yyVal = new PtrtointValue ((TypedValue)yyVals[-3+yyTop], (LType)yyVals[-1+yyTop]);
    }
  break;
case 253:
#line 791 "Iril/IR/IR.jay"
  {
        yyVal = new VectorConstant ((List<TypedValue>)yyVals[-1+yyTop]);
    }
  break;
case 254:
#line 795 "Iril/IR/IR.jay"
  {
        yyVal = new ArrayConstant ((List<TypedValue>)yyVals[-1+yyTop]);
    }
  break;
case 255:
#line 799 "Iril/IR/IR.jay"
  {
        yyVal = new StructureConstant ((List<TypedValue>)yyVals[-1+yyTop]);
    }
  break;
case 256:
#line 803 "Iril/IR/IR.jay"
  {
        yyVal = new StructureConstant ((List<TypedValue>)yyVals[-2+yyTop]);
    }
  break;
case 257:
#line 807 "Iril/IR/IR.jay"
  {
        yyVal = new AddrSpaceCastValue ((TypedValue)yyVals[-3+yyTop], (LType)yyVals[-1+yyTop]);
    }
  break;
case 259:
#line 815 "Iril/IR/IR.jay"
  { yyVal = NullConstant.Null; }
  break;
case 260:
#line 816 "Iril/IR/IR.jay"
  { yyVal = new FloatConstant ((double)yyVals[0+yyTop]); }
  break;
case 261:
#line 817 "Iril/IR/IR.jay"
  { yyVal = new IntegerConstant ((BigInteger)yyVals[0+yyTop]); }
  break;
case 262:
#line 818 "Iril/IR/IR.jay"
  { yyVal = new HexIntegerConstant ((BigInteger)yyVals[0+yyTop]); }
  break;
case 263:
#line 819 "Iril/IR/IR.jay"
  { yyVal = BooleanConstant.True; }
  break;
case 264:
#line 820 "Iril/IR/IR.jay"
  { yyVal = BooleanConstant.False; }
  break;
case 265:
#line 821 "Iril/IR/IR.jay"
  { yyVal = UndefinedConstant.Undefined; }
  break;
case 266:
#line 822 "Iril/IR/IR.jay"
  { yyVal = ZeroConstant.Zero; }
  break;
case 267:
#line 823 "Iril/IR/IR.jay"
  { yyVal = new BytesConstant ((Symbol)yyVals[0+yyTop]); }
  break;
case 268:
#line 830 "Iril/IR/IR.jay"
  {
        yyVal = new LabelValue ((LocalSymbol)yyVals[0+yyTop]);
    }
  break;
case 269:
#line 837 "Iril/IR/IR.jay"
  {
        yyVal = new TypedValue ((LType)yyVals[-1+yyTop], (Value)yyVals[0+yyTop]);
    }
  break;
case 270:
#line 841 "Iril/IR/IR.jay"
  {
        yyVal = new TypedValue (VoidType.Void, VoidValue.Void);
    }
  break;
case 271:
#line 848 "Iril/IR/IR.jay"
  {
        yyVal = new TypedValue ((LType)yyVals[-1+yyTop], (Value)yyVals[0+yyTop]);
    }
  break;
case 272:
#line 855 "Iril/IR/IR.jay"
  {
        yyVal = NewList ((TypedValue)yyVals[0+yyTop]);
    }
  break;
case 273:
#line 859 "Iril/IR/IR.jay"
  {
        yyVal = ListAdd (yyVals[-2+yyTop], (TypedValue)yyVals[0+yyTop]);
    }
  break;
case 274:
#line 866 "Iril/IR/IR.jay"
  {
        yyVal = new TypedConstant ((LType)yyVals[-1+yyTop], (Constant)yyVals[0+yyTop]);
    }
  break;
case 276:
#line 874 "Iril/IR/IR.jay"
  {
        yyVal = yyVals[0+yyTop];
    }
  break;
case 277:
#line 881 "Iril/IR/IR.jay"
  {
        yyVal = NewList ((TypedValue)yyVals[0+yyTop]);
    }
  break;
case 278:
#line 885 "Iril/IR/IR.jay"
  {
        yyVal = ListAdd (yyVals[-2+yyTop], (TypedValue)yyVals[0+yyTop]);
    }
  break;
case 280:
#line 896 "Iril/IR/IR.jay"
  {
        yyVal = NewList ((Value)yyVals[0+yyTop]);
    }
  break;
case 281:
#line 900 "Iril/IR/IR.jay"
  {
        yyVal = ListAdd (yyVals[-2+yyTop], (Value)yyVals[0+yyTop]);
    }
  break;
case 282:
#line 907 "Iril/IR/IR.jay"
  {
        yyVal = NewList ((Block)yyVals[0+yyTop]);
    }
  break;
case 283:
#line 911 "Iril/IR/IR.jay"
  {
        yyVal = ListAdd (yyVals[-1+yyTop], (Block)yyVals[0+yyTop]);
    }
  break;
case 284:
#line 918 "Iril/IR/IR.jay"
  {
        yyVal = yyVals[0+yyTop];
    }
  break;
case 285:
  case_285();
  break;
case 286:
#line 928 "Iril/IR/IR.jay"
  {
        yyVal = yyVals[0+yyTop];
    }
  break;
case 287:
#line 935 "Iril/IR/IR.jay"
  {
        yyVal = new Block (LocalSymbol.None, (List<Assignment>)yyVals[-1+yyTop], (Assignment)yyVals[0+yyTop]);
    }
  break;
case 288:
#line 939 "Iril/IR/IR.jay"
  {
        yyVal = new Block (LocalSymbol.None, (List<Assignment>)yyVals[-2+yyTop], (Assignment)yyVals[-1+yyTop]);
    }
  break;
case 289:
#line 943 "Iril/IR/IR.jay"
  {
        yyVal = new Block (LocalSymbol.None, Enumerable.Empty<Assignment>(), (Assignment)yyVals[0+yyTop]);
    }
  break;
case 290:
#line 947 "Iril/IR/IR.jay"
  {
        yyVal = new Block (LocalSymbol.None, Enumerable.Empty<Assignment>(), (Assignment)yyVals[-1+yyTop]);
    }
  break;
case 291:
#line 954 "Iril/IR/IR.jay"
  {
        yyVal = NewList ((Assignment)yyVals[0+yyTop]);
    }
  break;
case 292:
#line 958 "Iril/IR/IR.jay"
  {
        yyVal = ListAdd (yyVals[-1+yyTop], (Assignment)yyVals[0+yyTop]);
    }
  break;
case 293:
#line 965 "Iril/IR/IR.jay"
  {
        yyVal = new Assignment ((Instruction)yyVals[0+yyTop]);
    }
  break;
case 294:
#line 969 "Iril/IR/IR.jay"
  {
        yyVal = new Assignment ((Instruction)yyVals[-1+yyTop], (SymbolTable<MetaSymbol>)yyVals[0+yyTop]);
    }
  break;
case 295:
#line 973 "Iril/IR/IR.jay"
  {
        yyVal = new Assignment ((LocalSymbol)yyVals[-2+yyTop], (Instruction)yyVals[0+yyTop]);
    }
  break;
case 296:
#line 977 "Iril/IR/IR.jay"
  {
        yyVal = new Assignment ((LocalSymbol)yyVals[-3+yyTop], (Instruction)yyVals[-1+yyTop], (SymbolTable<MetaSymbol>)yyVals[0+yyTop]);
    }
  break;
case 298:
#line 985 "Iril/IR/IR.jay"
  { yyVal = yyVals[-1+yyTop]; }
  break;
case 299:
#line 986 "Iril/IR/IR.jay"
  { yyVal = Enumerable.Empty<Argument> (); }
  break;
case 300:
#line 993 "Iril/IR/IR.jay"
  {
        yyVal = NewList ((Argument)yyVals[0+yyTop]);
    }
  break;
case 301:
#line 997 "Iril/IR/IR.jay"
  {
        yyVal = ListAdd (yyVals[-2+yyTop], (Argument)yyVals[0+yyTop]);
    }
  break;
case 302:
#line 1004 "Iril/IR/IR.jay"
  {
        yyVal = new Argument ((LType)yyVals[-1+yyTop], (Value)yyVals[0+yyTop], (ParameterAttributes)0);
    }
  break;
case 303:
#line 1008 "Iril/IR/IR.jay"
  {
        yyVal = new Argument ((LType)yyVals[-2+yyTop], (Value)yyVals[0+yyTop], ParameterAttributes.NonNull);
    }
  break;
case 304:
#line 1012 "Iril/IR/IR.jay"
  {
        yyVal = new Argument ((LType)yyVals[-1+yyTop], (Value)yyVals[0+yyTop], (ParameterAttributes)0);
    }
  break;
case 305:
#line 1016 "Iril/IR/IR.jay"
  {
        yyVal = new Argument (IntegerType.I32, new MetaValue ((MetaSymbol)yyVals[0+yyTop]), (ParameterAttributes)0);
    }
  break;
case 306:
#line 1020 "Iril/IR/IR.jay"
  {
        yyVal = new Argument (IntegerType.I32, new MetaValue ((MetaSymbol)yyVals[-2+yyTop]), (ParameterAttributes)0);
    }
  break;
case 307:
#line 1024 "Iril/IR/IR.jay"
  {
        yyVal = new Argument (IntegerType.I32, new MetaValue ((MetaSymbol)yyVals[-3+yyTop]), (ParameterAttributes)0);
    }
  break;
case 309:
#line 1032 "Iril/IR/IR.jay"
  {
        yyVal = new GlobalValue ((GlobalSymbol)yyVals[0+yyTop]);
    }
  break;
case 310:
#line 1036 "Iril/IR/IR.jay"
  {
        yyVal = new LocalValue ((LocalSymbol)yyVals[0+yyTop]);
    }
  break;
case 311:
#line 1040 "Iril/IR/IR.jay"
  {
        yyVal = new SymbolValue ((Symbol)yyVals[0+yyTop]);
    }
  break;
case 312:
#line 1044 "Iril/IR/IR.jay"
  {
        yyVal = new IntToPointerValue ((TypedValue)yyVals[-3+yyTop], (LType)yyVals[-1+yyTop]);
    }
  break;
case 313:
#line 1048 "Iril/IR/IR.jay"
  {
        yyVal = new GetElementPointerValue ((LType)yyVals[-5+yyTop], (TypedValue)yyVals[-3+yyTop], (List<TypedValue>)yyVals[-1+yyTop]);
    }
  break;
case 314:
#line 1052 "Iril/IR/IR.jay"
  {
        yyVal = new GetElementPointerValue ((LType)yyVals[-5+yyTop], (TypedValue)yyVals[-3+yyTop], (List<TypedValue>)yyVals[-1+yyTop]);
    }
  break;
case 315:
#line 1056 "Iril/IR/IR.jay"
  {
        yyVal = new BitcastValue ((TypedValue)yyVals[-3+yyTop], (LType)yyVals[-1+yyTop]);
    }
  break;
case 316:
#line 1060 "Iril/IR/IR.jay"
  {
        yyVal = new PtrtointValue ((TypedValue)yyVals[-3+yyTop], (LType)yyVals[-1+yyTop]);
    }
  break;
case 324:
#line 1080 "Iril/IR/IR.jay"
  {
        yyVal = NewList ((PhiValue)yyVals[0+yyTop]);
    }
  break;
case 325:
#line 1084 "Iril/IR/IR.jay"
  {
        yyVal = ListAdd (yyVals[-2+yyTop], (PhiValue)yyVals[0+yyTop]);
    }
  break;
case 326:
#line 1090 "Iril/IR/IR.jay"
  {
        yyVal = new PhiValue ((Value)yyVals[-3+yyTop], (Value)yyVals[-1+yyTop]);
    }
  break;
case 327:
#line 1097 "Iril/IR/IR.jay"
  {
        yyVal = NewList ((SwitchCase)yyVals[0+yyTop]);
    }
  break;
case 328:
#line 1101 "Iril/IR/IR.jay"
  {
        yyVal = ListAdd (yyVals[-1+yyTop], (SwitchCase)yyVals[0+yyTop]);
    }
  break;
case 329:
#line 1108 "Iril/IR/IR.jay"
  {
        yyVal = new SwitchCase ((TypedConstant)yyVals[-2+yyTop], (LabelValue)yyVals[0+yyTop]);
    }
  break;
case 337:
#line 1128 "Iril/IR/IR.jay"
  { yyVal = AtomicConstraint.SequentiallyConsistent; }
  break;
case 338:
#line 1135 "Iril/IR/IR.jay"
  {
        yyVal = new InlineAssemblyValue ((string)yyVals[-2+yyTop], (string)yyVals[0+yyTop]);
    }
  break;
case 339:
#line 1142 "Iril/IR/IR.jay"
  {
        yyVal = new Assignment (LocalSymbol.None, (Instruction)yyVals[0+yyTop]);
    }
  break;
case 340:
#line 1146 "Iril/IR/IR.jay"
  {
        yyVal = new Assignment ((LocalSymbol)yyVals[-2+yyTop], (Instruction)yyVals[0+yyTop]);
    }
  break;
case 341:
#line 1153 "Iril/IR/IR.jay"
  {
        yyVal = new UnconditionalBrInstruction ((LabelValue)yyVals[0+yyTop]);
    }
  break;
case 342:
#line 1157 "Iril/IR/IR.jay"
  {
        yyVal = new ConditionalBrInstruction ((Value)yyVals[-4+yyTop], (LabelValue)yyVals[-2+yyTop], (LabelValue)yyVals[0+yyTop]);
    }
  break;
case 343:
#line 1161 "Iril/IR/IR.jay"
  {
        yyVal = new ResumeInstruction ((TypedValue)yyVals[0+yyTop]);
    }
  break;
case 344:
#line 1165 "Iril/IR/IR.jay"
  {
        yyVal = new RetInstruction ((TypedValue)yyVals[0+yyTop]);
    }
  break;
case 345:
#line 1169 "Iril/IR/IR.jay"
  {
        yyVal = new SwitchInstruction ((TypedValue)yyVals[-5+yyTop], (LabelValue)yyVals[-3+yyTop], (List<SwitchCase>)yyVals[-1+yyTop]);
    }
  break;
case 346:
#line 1173 "Iril/IR/IR.jay"
  {
        yyVal = UnreachableInstruction.Unreachable;
    }
  break;
case 348:
#line 1181 "Iril/IR/IR.jay"
  {
        yyVal = new InvokeInstruction ((LType)yyVals[-6+yyTop], (Value)yyVals[-5+yyTop], (IEnumerable<Argument>)yyVals[-4+yyTop], (LabelValue)yyVals[-2+yyTop], (LabelValue)yyVals[0+yyTop]);
    }
  break;
case 349:
#line 1185 "Iril/IR/IR.jay"
  {
        yyVal = new InvokeInstruction ((LType)yyVals[-6+yyTop], (Value)yyVals[-5+yyTop], (IEnumerable<Argument>)yyVals[-4+yyTop], (LabelValue)yyVals[-2+yyTop], (LabelValue)yyVals[0+yyTop]);
    }
  break;
case 350:
#line 1189 "Iril/IR/IR.jay"
  {
        yyVal = new InvokeInstruction ((LType)yyVals[-6+yyTop], (Value)yyVals[-5+yyTop], (IEnumerable<Argument>)yyVals[-4+yyTop], (LabelValue)yyVals[-2+yyTop], (LabelValue)yyVals[0+yyTop]);
    }
  break;
case 351:
#line 1193 "Iril/IR/IR.jay"
  {
        yyVal = new InvokeInstruction ((LType)yyVals[-6+yyTop], (Value)yyVals[-5+yyTop], (IEnumerable<Argument>)yyVals[-4+yyTop], (LabelValue)yyVals[-2+yyTop], (LabelValue)yyVals[0+yyTop]);
    }
  break;
case 352:
#line 1200 "Iril/IR/IR.jay"
  {
        yyVal = false;
    }
  break;
case 353:
#line 1204 "Iril/IR/IR.jay"
  {
        yyVal = true;
    }
  break;
case 354:
#line 1211 "Iril/IR/IR.jay"
  {
        yyVal = new AddInstruction ((LType)yyVals[-3+yyTop], (Value)yyVals[-2+yyTop], (Value)yyVals[0+yyTop], isAtomic: false);
    }
  break;
case 355:
#line 1215 "Iril/IR/IR.jay"
  {
        yyVal = new AddInstruction ((LType)yyVals[-3+yyTop], (Value)yyVals[-2+yyTop], (Value)yyVals[0+yyTop], isAtomic: false);
    }
  break;
case 356:
#line 1219 "Iril/IR/IR.jay"
  {
        yyVal = new AddInstruction ((LType)yyVals[-8+yyTop], (Value)yyVals[-7+yyTop], (Value)yyVals[-4+yyTop], isAtomic: true);
    }
  break;
case 357:
#line 1223 "Iril/IR/IR.jay"
  {
        yyVal = new AllocaInstruction ((LType)yyVals[-3+yyTop], (int)(BigInteger)yyVals[0+yyTop], numElements: (TypedValue)null);
    }
  break;
case 358:
#line 1227 "Iril/IR/IR.jay"
  {
        yyVal = new AllocaInstruction ((LType)yyVals[-5+yyTop], (int)(BigInteger)yyVals[0+yyTop], numElements: (TypedValue)yyVals[-3+yyTop]);
    }
  break;
case 359:
#line 1231 "Iril/IR/IR.jay"
  {
        yyVal = new AndInstruction ((LType)yyVals[-3+yyTop], (Value)yyVals[-2+yyTop], (Value)yyVals[0+yyTop]);
    }
  break;
case 360:
#line 1235 "Iril/IR/IR.jay"
  {
        yyVal = new AshrInstruction ((LType)yyVals[-3+yyTop], (Value)yyVals[-2+yyTop], (Value)yyVals[0+yyTop], false);
    }
  break;
case 361:
#line 1239 "Iril/IR/IR.jay"
  {
        yyVal = new AshrInstruction ((LType)yyVals[-3+yyTop], (Value)yyVals[-2+yyTop], (Value)yyVals[0+yyTop], true);
    }
  break;
case 362:
#line 1243 "Iril/IR/IR.jay"
  {
        yyVal = new BitcastInstruction ((TypedValue)yyVals[-2+yyTop], (LType)yyVals[0+yyTop]);
    }
  break;
case 363:
#line 1247 "Iril/IR/IR.jay"
  {
        yyVal = new CallInstruction ((LType)yyVals[-2+yyTop], (Value)yyVals[-1+yyTop], (IEnumerable<Argument>)yyVals[0+yyTop], false);
    }
  break;
case 364:
#line 1251 "Iril/IR/IR.jay"
  {
        yyVal = new CallInstruction ((LType)yyVals[-3+yyTop], (Value)yyVals[-2+yyTop], (IEnumerable<Argument>)yyVals[-1+yyTop], false);
    }
  break;
case 365:
#line 1255 "Iril/IR/IR.jay"
  {
        yyVal = new CallInstruction ((LType)yyVals[-2+yyTop], (Value)yyVals[-1+yyTop], (IEnumerable<Argument>)yyVals[0+yyTop], false);
    }
  break;
case 366:
#line 1259 "Iril/IR/IR.jay"
  {
        yyVal = new CallInstruction ((LType)yyVals[-3+yyTop], (Value)yyVals[-2+yyTop], (IEnumerable<Argument>)yyVals[-1+yyTop], false);
    }
  break;
case 367:
#line 1263 "Iril/IR/IR.jay"
  {
        yyVal = new CallInstruction ((LType)yyVals[-2+yyTop], (Value)yyVals[-1+yyTop], (IEnumerable<Argument>)yyVals[0+yyTop], false);
    }
  break;
case 368:
#line 1267 "Iril/IR/IR.jay"
  {
        yyVal = new CallInstruction ((LType)yyVals[-3+yyTop], (Value)yyVals[-2+yyTop], (IEnumerable<Argument>)yyVals[-1+yyTop], false);
    }
  break;
case 369:
#line 1271 "Iril/IR/IR.jay"
  {
        yyVal = new CallInstruction ((LType)yyVals[-3+yyTop], (Value)yyVals[-2+yyTop], (IEnumerable<Argument>)yyVals[-1+yyTop], false);
    }
  break;
case 370:
#line 1275 "Iril/IR/IR.jay"
  {
        yyVal = new CallInstruction ((LType)yyVals[-2+yyTop], (Value)yyVals[-1+yyTop], (IEnumerable<Argument>)yyVals[0+yyTop], false);
    }
  break;
case 371:
#line 1279 "Iril/IR/IR.jay"
  {
        yyVal = new CallInstruction ((LType)yyVals[-3+yyTop], (Value)yyVals[-2+yyTop], (IEnumerable<Argument>)yyVals[-1+yyTop], false);
    }
  break;
case 372:
#line 1283 "Iril/IR/IR.jay"
  {
        yyVal = new CallInstruction ((LType)yyVals[-3+yyTop], (Value)yyVals[-2+yyTop], (IEnumerable<Argument>)yyVals[-1+yyTop], true);
    }
  break;
case 373:
#line 1287 "Iril/IR/IR.jay"
  {
        yyVal = new CallInstruction ((LType)yyVals[-3+yyTop], (Value)yyVals[-2+yyTop], (IEnumerable<Argument>)yyVals[-1+yyTop], true);
    }
  break;
case 374:
#line 1291 "Iril/IR/IR.jay"
  {
        yyVal = new CallInstruction ((LType)yyVals[-2+yyTop], (Value)yyVals[-1+yyTop], (IEnumerable<Argument>)yyVals[0+yyTop], true);
    }
  break;
case 375:
#line 1295 "Iril/IR/IR.jay"
  {
        yyVal = new CallInstruction ((LType)yyVals[-3+yyTop], (Value)yyVals[-2+yyTop], (IEnumerable<Argument>)yyVals[-1+yyTop], true);
    }
  break;
case 376:
#line 1299 "Iril/IR/IR.jay"
  {
        yyVal = new CallInstruction ((LType)yyVals[-2+yyTop], (Value)yyVals[-1+yyTop], (IEnumerable<Argument>)yyVals[0+yyTop], true);
    }
  break;
case 377:
#line 1303 "Iril/IR/IR.jay"
  {
        yyVal = new CallInstruction ((LType)yyVals[-2+yyTop], (Value)yyVals[-1+yyTop], (IEnumerable<Argument>)yyVals[0+yyTop], true);
    }
  break;
case 378:
#line 1307 "Iril/IR/IR.jay"
  {
        yyVal = new CallInstruction ((LType)yyVals[-3+yyTop], (Value)yyVals[-2+yyTop], (IEnumerable<Argument>)yyVals[-1+yyTop], true);
    }
  break;
case 379:
#line 1311 "Iril/IR/IR.jay"
  {
        yyVal = new CallInstruction ((LType)yyVals[-2+yyTop], (Value)yyVals[-1+yyTop], (IEnumerable<Argument>)yyVals[0+yyTop], true);
    }
  break;
case 380:
#line 1315 "Iril/IR/IR.jay"
  {
        yyVal = new CallInstruction ((LType)yyVals[-3+yyTop], (Value)yyVals[-2+yyTop], (IEnumerable<Argument>)yyVals[-1+yyTop], true);
    }
  break;
case 381:
#line 1319 "Iril/IR/IR.jay"
  {
        yyVal = new CallInstruction ((LType)yyVals[-3+yyTop], (Value)yyVals[-2+yyTop], (IEnumerable<Argument>)yyVals[-1+yyTop], true);
    }
  break;
case 382:
#line 1323 "Iril/IR/IR.jay"
  {
        yyVal = new ExtractElementInstruction ((TypedValue)yyVals[-2+yyTop], (TypedValue)yyVals[0+yyTop]);
    }
  break;
case 383:
#line 1327 "Iril/IR/IR.jay"
  {
        yyVal = new ExtractValueInstruction ((TypedValue)yyVals[-2+yyTop], (List<Value>)yyVals[0+yyTop]);
    }
  break;
case 384:
#line 1331 "Iril/IR/IR.jay"
  {
        yyVal = new FaddInstruction ((LType)yyVals[-3+yyTop], (Value)yyVals[-2+yyTop], (Value)yyVals[0+yyTop]);
    }
  break;
case 385:
#line 1335 "Iril/IR/IR.jay"
  {
        yyVal = new FcmpInstruction ((FcmpCondition)yyVals[-4+yyTop], (LType)yyVals[-3+yyTop], (Value)yyVals[-2+yyTop], (Value)yyVals[0+yyTop]);
    }
  break;
case 386:
#line 1339 "Iril/IR/IR.jay"
  {
        yyVal = new FdivInstruction ((LType)yyVals[-3+yyTop], (Value)yyVals[-2+yyTop], (Value)yyVals[0+yyTop]);
    }
  break;
case 387:
#line 1343 "Iril/IR/IR.jay"
  {
        yyVal = new FenceInstruction ((AtomicConstraint)yyVals[0+yyTop]);
    }
  break;
case 388:
#line 1347 "Iril/IR/IR.jay"
  {
        yyVal = new FmulInstruction ((LType)yyVals[-3+yyTop], (Value)yyVals[-2+yyTop], (Value)yyVals[0+yyTop]);
    }
  break;
case 389:
#line 1351 "Iril/IR/IR.jay"
  {
        yyVal = new FpextInstruction ((TypedValue)yyVals[-2+yyTop], (LType)yyVals[0+yyTop]);
    }
  break;
case 390:
#line 1355 "Iril/IR/IR.jay"
  {
        yyVal = new FptouiInstruction ((TypedValue)yyVals[-2+yyTop], (LType)yyVals[0+yyTop]);
    }
  break;
case 391:
#line 1359 "Iril/IR/IR.jay"
  {
        yyVal = new FptosiInstruction ((TypedValue)yyVals[-2+yyTop], (LType)yyVals[0+yyTop]);
    }
  break;
case 392:
#line 1363 "Iril/IR/IR.jay"
  {
        yyVal = new FptruncInstruction ((TypedValue)yyVals[-2+yyTop], (LType)yyVals[0+yyTop]);
    }
  break;
case 393:
#line 1367 "Iril/IR/IR.jay"
  {
        yyVal = new FsubInstruction ((LType)yyVals[-3+yyTop], (Value)yyVals[-2+yyTop], (Value)yyVals[0+yyTop]);
    }
  break;
case 394:
#line 1371 "Iril/IR/IR.jay"
  {
        yyVal = new GetElementPointerInstruction ((LType)yyVals[-4+yyTop], (TypedValue)yyVals[-2+yyTop], (List<TypedValue>)yyVals[0+yyTop]);
    }
  break;
case 395:
#line 1375 "Iril/IR/IR.jay"
  {
        yyVal = new GetElementPointerInstruction ((LType)yyVals[-4+yyTop], (TypedValue)yyVals[-2+yyTop], (List<TypedValue>)yyVals[0+yyTop]);
    }
  break;
case 396:
#line 1379 "Iril/IR/IR.jay"
  {
        yyVal = new GetElementPointerInstruction ((LType)yyVals[-4+yyTop], (TypedValue)yyVals[-2+yyTop], (List<TypedValue>)yyVals[0+yyTop]);
    }
  break;
case 397:
#line 1383 "Iril/IR/IR.jay"
  {
        yyVal = new GetElementPointerInstruction ((LType)yyVals[-4+yyTop], (TypedValue)yyVals[-2+yyTop], (List<TypedValue>)yyVals[0+yyTop]);
    }
  break;
case 398:
#line 1387 "Iril/IR/IR.jay"
  {
        yyVal = new IcmpInstruction ((IcmpCondition)yyVals[-4+yyTop], (LType)yyVals[-3+yyTop], (Value)yyVals[-2+yyTop], (Value)yyVals[0+yyTop]);
    }
  break;
case 399:
#line 1391 "Iril/IR/IR.jay"
  {
        yyVal = new InsertElementInstruction ((TypedValue)yyVals[-4+yyTop], (TypedValue)yyVals[-2+yyTop], (TypedValue)yyVals[0+yyTop]);
    }
  break;
case 400:
#line 1395 "Iril/IR/IR.jay"
  {
        yyVal = new InsertValueInstruction ((TypedValue)yyVals[-4+yyTop], (TypedValue)yyVals[-2+yyTop], (List<Value>)yyVals[0+yyTop]);
    }
  break;
case 401:
#line 1399 "Iril/IR/IR.jay"
  {
        yyVal = new InttoptrInstruction ((TypedValue)yyVals[-2+yyTop], (LType)yyVals[0+yyTop]);
    }
  break;
case 402:
#line 1403 "Iril/IR/IR.jay"
  {
        yyVal = new LandingPadInstruction ((LType)yyVals[-1+yyTop]);
    }
  break;
case 403:
#line 1407 "Iril/IR/IR.jay"
  {
        yyVal = new LandingPadInstruction ((LType)yyVals[-2+yyTop], (TypedValue)yyVals[0+yyTop]);
    }
  break;
case 404:
#line 1411 "Iril/IR/IR.jay"
  {
        yyVal = new LoadInstruction ((LType)yyVals[-5+yyTop], (TypedValue)yyVals[-3+yyTop], isVolatile: false, isAtomic: false);
    }
  break;
case 405:
#line 1415 "Iril/IR/IR.jay"
  {
        yyVal = new LoadInstruction ((LType)yyVals[-5+yyTop], (TypedValue)yyVals[-3+yyTop], isVolatile: false, isAtomic: true);
    }
  break;
case 406:
#line 1419 "Iril/IR/IR.jay"
  {
        yyVal = new LoadInstruction ((LType)yyVals[-5+yyTop], (TypedValue)yyVals[-3+yyTop], isVolatile: true, isAtomic: false);
    }
  break;
case 407:
#line 1423 "Iril/IR/IR.jay"
  {
        yyVal = new LoadInstruction ((LType)yyVals[-5+yyTop], (TypedValue)yyVals[-3+yyTop], isVolatile: true, isAtomic: true);
    }
  break;
case 408:
#line 1427 "Iril/IR/IR.jay"
  {
        yyVal = new LoadInstruction ((LType)yyVals[-6+yyTop], (TypedValue)yyVals[-4+yyTop], isVolatile: true, isAtomic: true);
    }
  break;
case 409:
#line 1431 "Iril/IR/IR.jay"
  {
        yyVal = new LshrInstruction ((LType)yyVals[-3+yyTop], (Value)yyVals[-2+yyTop], (Value)yyVals[0+yyTop], false);
    }
  break;
case 410:
#line 1435 "Iril/IR/IR.jay"
  {
        yyVal = new LshrInstruction ((LType)yyVals[-3+yyTop], (Value)yyVals[-2+yyTop], (Value)yyVals[0+yyTop], true);
    }
  break;
case 411:
#line 1439 "Iril/IR/IR.jay"
  {
        yyVal = new OrInstruction ((LType)yyVals[-3+yyTop], (Value)yyVals[-2+yyTop], (Value)yyVals[0+yyTop]);
    }
  break;
case 412:
#line 1443 "Iril/IR/IR.jay"
  {
        yyVal = new MultiplyInstruction ((LType)yyVals[-3+yyTop], (Value)yyVals[-2+yyTop], (Value)yyVals[0+yyTop]);
    }
  break;
case 413:
#line 1447 "Iril/IR/IR.jay"
  {
        yyVal = new MultiplyInstruction ((LType)yyVals[-3+yyTop], (Value)yyVals[-2+yyTop], (Value)yyVals[0+yyTop]);
    }
  break;
case 414:
#line 1451 "Iril/IR/IR.jay"
  {
        yyVal = new PhiInstruction ((LType)yyVals[-1+yyTop], (List<PhiValue>)yyVals[0+yyTop]);
    }
  break;
case 415:
#line 1455 "Iril/IR/IR.jay"
  {
        yyVal = new PtrtointInstruction ((TypedValue)yyVals[-2+yyTop], (LType)yyVals[0+yyTop]);
    }
  break;
case 416:
#line 1459 "Iril/IR/IR.jay"
  {
        yyVal = new SdivInstruction ((LType)yyVals[-3+yyTop], (Value)yyVals[-2+yyTop], (Value)yyVals[0+yyTop]);
    }
  break;
case 417:
#line 1463 "Iril/IR/IR.jay"
  {
        yyVal = new SdivInstruction ((LType)yyVals[-3+yyTop], (Value)yyVals[-2+yyTop], (Value)yyVals[0+yyTop]);
    }
  break;
case 418:
#line 1467 "Iril/IR/IR.jay"
  {
        yyVal = new SelectInstruction ((LType)yyVals[-5+yyTop], (Value)yyVals[-4+yyTop], (TypedValue)yyVals[-2+yyTop], (TypedValue)yyVals[0+yyTop]);
    }
  break;
case 419:
#line 1471 "Iril/IR/IR.jay"
  {
        yyVal = new SextInstruction ((TypedValue)yyVals[-2+yyTop], (LType)yyVals[0+yyTop]);
    }
  break;
case 420:
#line 1475 "Iril/IR/IR.jay"
  {
        yyVal = new ShlInstruction ((LType)yyVals[-3+yyTop], (Value)yyVals[-2+yyTop], (Value)yyVals[0+yyTop]);
    }
  break;
case 421:
#line 1479 "Iril/IR/IR.jay"
  {
        yyVal = new ShlInstruction ((LType)yyVals[-3+yyTop], (Value)yyVals[-2+yyTop], (Value)yyVals[0+yyTop]);
    }
  break;
case 422:
#line 1483 "Iril/IR/IR.jay"
  {
        yyVal = new ShuffleVectorInstruction ((TypedValue)yyVals[-4+yyTop], (TypedValue)yyVals[-2+yyTop], (TypedValue)yyVals[0+yyTop]);
    }
  break;
case 423:
#line 1487 "Iril/IR/IR.jay"
  {
        yyVal = new SitofpInstruction ((TypedValue)yyVals[-2+yyTop], (LType)yyVals[0+yyTop]);
    }
  break;
case 424:
#line 1491 "Iril/IR/IR.jay"
  {
        yyVal = new SremInstruction ((LType)yyVals[-3+yyTop], (Value)yyVals[-2+yyTop], (Value)yyVals[0+yyTop]);
    }
  break;
case 425:
#line 1495 "Iril/IR/IR.jay"
  {
        yyVal = new StoreInstruction (value: (TypedValue)yyVals[-5+yyTop], pointer: (TypedValue)yyVals[-3+yyTop], isVolatile: false);
    }
  break;
case 426:
#line 1499 "Iril/IR/IR.jay"
  {
        yyVal = new StoreInstruction (value: (TypedValue)yyVals[-5+yyTop], pointer: (TypedValue)yyVals[-3+yyTop], isVolatile: true);
    }
  break;
case 427:
#line 1503 "Iril/IR/IR.jay"
  {
        yyVal = new SubInstruction ((LType)yyVals[-3+yyTop], (Value)yyVals[-2+yyTop], (Value)yyVals[0+yyTop], isAtomic: false);
    }
  break;
case 428:
#line 1507 "Iril/IR/IR.jay"
  {
        yyVal = new SubInstruction ((LType)yyVals[-3+yyTop], (Value)yyVals[-2+yyTop], (Value)yyVals[0+yyTop], isAtomic: false);
    }
  break;
case 429:
#line 1511 "Iril/IR/IR.jay"
  {
        yyVal = new SubInstruction ((LType)yyVals[-8+yyTop], (Value)yyVals[-7+yyTop], (Value)yyVals[-4+yyTop], isAtomic: true);
    }
  break;
case 430:
#line 1515 "Iril/IR/IR.jay"
  {
        yyVal = new TruncInstruction ((TypedValue)yyVals[-2+yyTop], (LType)yyVals[0+yyTop]);
    }
  break;
case 431:
#line 1519 "Iril/IR/IR.jay"
  {
        yyVal = new UdivInstruction ((LType)yyVals[-3+yyTop], (Value)yyVals[-2+yyTop], (Value)yyVals[0+yyTop]);
    }
  break;
case 432:
#line 1523 "Iril/IR/IR.jay"
  {
        yyVal = new UitofpInstruction ((TypedValue)yyVals[-2+yyTop], (LType)yyVals[0+yyTop]);
    }
  break;
case 433:
#line 1527 "Iril/IR/IR.jay"
  {
        yyVal = new UremInstruction ((LType)yyVals[-3+yyTop], (Value)yyVals[-2+yyTop], (Value)yyVals[0+yyTop]);
    }
  break;
case 434:
#line 1531 "Iril/IR/IR.jay"
  {
        yyVal = new XorInstruction ((LType)yyVals[-3+yyTop], (Value)yyVals[-2+yyTop], (Value)yyVals[0+yyTop]);
    }
  break;
case 435:
#line 1535 "Iril/IR/IR.jay"
  {
        yyVal = new ZextInstruction ((TypedValue)yyVals[-2+yyTop], (LType)yyVals[0+yyTop]);
    }
  break;
#line default
        }
        yyTop -= yyLen[yyN];
        yyState = yyStates[yyTop];
        int yyM = yyLhs[yyN];
        if (yyState == 0 && yyM == 0) {
//t          if (debug != null) debug.shift(0, yyFinal);
          yyState = yyFinal;
          if (yyToken < 0) {
            yyToken = yyLex.advance() ? yyLex.token() : 0;
//t            if (debug != null)
//t               debug.lex(yyState, yyToken,yyname(yyToken), yyLex.value());
          }
          if (yyToken == 0) {
//t            if (debug != null) debug.accept(yyVal);
            return yyVal;
          }
          goto continue_yyLoop;
        }
        if (((yyN = yyGindex[yyM]) != 0) && ((yyN += yyState) >= 0)
            && (yyN < yyTable.Length) && (yyCheck[yyN] == yyState))
          yyState = yyTable[yyN];
        else
          yyState = yyDgoto[yyM];
//t        if (debug != null) debug.shift(yyStates[yyTop], yyState);
	 goto continue_yyLoop;
      continue_yyDiscarded: ;	// implements the named-loop continue: 'continue yyDiscarded'
      }
    continue_yyLoop: ;		// implements the named-loop continue: 'continue yyLoop'
    }
  }

/*
 All more than 3 lines long rules are wrapped into a method
*/
void case_9()
#line 83 "Iril/IR/IR.jay"
{
        var f = (FunctionDefinition)yyVals[0+yyTop];
        module.FunctionDefinitions[f.Symbol] = f;
    }

void case_10()
#line 88 "Iril/IR/IR.jay"
{
        var f = (FunctionDeclaration)yyVals[0+yyTop];
        module.FunctionDeclarations[f.Symbol] = f;
    }

void case_11()
#line 93 "Iril/IR/IR.jay"
{
        var g = (GlobalVariable)yyVals[0+yyTop];
        module.AddGlobalVariable(g);
    }

void case_16()
#line 108 "Iril/IR/IR.jay"
{
        var m = SymsAdd (yyVals[-1+yyTop], Symbol.Intern("_f"), yyVals[-3+yyTop]);
        module.Metadata[(Symbol)yyVals[-5+yyTop]] = m;
    }

void case_18()
#line 117 "Iril/IR/IR.jay"
{
        var m = SymsAdd (yyVals[-1+yyTop], Symbol.Intern("_f"), yyVals[-3+yyTop]);
        module.Metadata[(Symbol)yyVals[-6+yyTop]] = m;
    }

void case_60()
#line 274 "Iril/IR/IR.jay"
{
        var t = (Tuple<object, object>)yyVals[0+yyTop];
        yyVal = NewSyms (t.Item1, t.Item2);
    }

void case_61()
#line 279 "Iril/IR/IR.jay"
{
        var t = (Tuple<object, object>)yyVals[0+yyTop];
        yyVal = SymsAdd (yyVals[-2+yyTop], t.Item1, t.Item2);
    }

void case_130()
#line 484 "Iril/IR/IR.jay"
{
        var h = (Tuple<object, object>)yyVals[-6+yyTop];
        yyVal = new FunctionDefinition ((LType)h.Item2, (GlobalSymbol)yyVals[-5+yyTop], (IEnumerable<Parameter>)yyVals[-4+yyTop], (List<Block>)yyVals[-1+yyTop], isExternal: (bool)h.Item1);
    }

void case_131()
#line 489 "Iril/IR/IR.jay"
{
        var h = (Tuple<object, object>)yyVals[-7+yyTop];
        yyVal = new FunctionDefinition ((LType)h.Item2, (GlobalSymbol)yyVals[-6+yyTop], (IEnumerable<Parameter>)yyVals[-5+yyTop], (List<Block>)yyVals[-1+yyTop], isExternal: (bool)h.Item1, (SymbolTable<MetaSymbol>)yyVals[-3+yyTop]);
    }

void case_285()
#line 920 "Iril/IR/IR.jay"
{
        /* Named basic-block label (e.g. `entry:`). Record it as %<name> so*/
        /* `br label %<name>` targets resolve to this block.*/
        yyVal = ((Block)yyVals[0+yyTop]).WithSymbol ((LocalSymbol)Symbol.Intern ("%" + ((Symbol)yyVals[-2+yyTop]).Text));
    }

#line default
   static readonly short [] yyLhs  = {              -1,
    0,    1,    1,    2,    2,    2,    2,    2,    2,    2,
    2,    2,    2,    2,    2,    2,    2,    2,    6,    6,
    6,    6,    6,    6,    6,    6,    6,    6,    6,    6,
    6,    6,    6,    6,    6,    6,    6,    6,    6,    6,
    6,    6,    6,    6,    6,    6,   11,   11,   14,   10,
   10,   10,   10,   10,   10,   10,   10,   18,   16,    9,
    9,   19,   19,   19,   19,   19,   20,   23,   23,   24,
   25,   25,   25,   25,   25,   25,   17,   17,    8,    8,
    8,    8,    8,   27,   27,   27,    7,    7,   29,   29,
   29,   29,   29,   29,   29,   29,   29,   29,   29,   29,
   29,    3,    3,    3,   30,   30,   31,   31,   12,   12,
   12,   12,   12,   12,   12,   12,   12,   12,   12,   12,
   12,   12,   34,   33,   33,   32,   32,   35,   35,    4,
    4,   38,   38,   38,   38,   38,   38,   38,   38,   38,
   38,   38,   36,   36,   36,   36,   36,   36,   43,   43,
   43,   43,   43,   43,   43,   41,   47,   47,    5,    5,
    5,    5,    5,    5,    5,    5,   48,   48,   48,   37,
   37,   49,   49,   50,   50,   50,   50,   50,   50,   45,
   45,   42,   42,   42,   42,   42,   42,   42,   42,   42,
   42,   42,   42,   42,   42,   42,   42,   42,   42,   42,
   42,   42,   51,   51,   52,   52,   15,   15,   15,   15,
   46,   46,   40,   40,   53,   54,   54,   54,   54,   54,
   54,   54,   54,   54,   54,   55,   55,   55,   55,   55,
   55,   55,   55,   55,   55,   55,   55,   55,   55,   55,
   55,   56,   13,   13,   57,   57,   57,   57,   57,   57,
   57,   57,   57,   57,   57,   57,   57,   60,   21,   21,
   21,   21,   21,   21,   21,   21,   21,   61,   28,   28,
   62,   59,   59,   26,   63,   63,   58,   58,   64,   65,
   65,   39,   39,   66,   66,   66,   67,   67,   67,   67,
   68,   68,   70,   70,   70,   70,   72,   73,   73,   74,
   74,   75,   75,   75,   75,   75,   75,   76,   76,   76,
   76,   76,   76,   76,   76,   76,   22,   22,   77,   77,
   77,   77,   77,   78,   78,   79,   80,   80,   81,   82,
   82,   83,   83,   44,   44,   44,   84,   85,   69,   69,
   86,   86,   86,   86,   86,   86,   86,   87,   87,   87,
   87,   88,   88,   71,   71,   71,   71,   71,   71,   71,
   71,   71,   71,   71,   71,   71,   71,   71,   71,   71,
   71,   71,   71,   71,   71,   71,   71,   71,   71,   71,
   71,   71,   71,   71,   71,   71,   71,   71,   71,   71,
   71,   71,   71,   71,   71,   71,   71,   71,   71,   71,
   71,   71,   71,   71,   71,   71,   71,   71,   71,   71,
   71,   71,   71,   71,   71,   71,   71,   71,   71,   71,
   71,   71,   71,   71,   71,   71,   71,   71,   71,   71,
   71,   71,   71,   71,   71,
  };
   static readonly short [] yyLen = {           2,
    1,    1,    2,    3,    4,    4,    4,    4,    1,    1,
    1,    6,    5,    5,    6,    6,    7,    7,    6,    5,
    8,    6,    9,    6,    6,    9,   10,   13,    9,   10,
   10,   10,   10,    7,   11,    6,    9,    9,   12,    9,
   10,   11,    9,   10,    8,    5,    1,    1,    1,    1,
    1,    1,    1,    1,    1,    1,    1,    1,    1,    1,
    3,    3,    3,    3,    6,    5,    1,    1,    3,    1,
    1,    1,    1,    1,    1,    1,    2,    3,    1,    2,
    3,    3,    3,    1,    1,    1,    1,    2,    1,    1,
    1,    1,    1,    1,    1,    3,    1,    1,    1,    1,
    4,    2,    3,    5,    1,    3,    1,    1,    1,    1,
    1,    1,    1,    1,    3,    4,    3,    5,    1,    2,
    5,    5,    4,    0,    4,    1,    3,    1,    1,    7,
    8,    1,    2,    4,    3,    5,    4,    1,    3,    2,
    4,    3,    2,    3,    3,    4,    5,    4,    1,    1,
    1,    1,    2,    3,    2,    2,    1,    2,    4,    5,
    6,    6,    7,    5,    6,    7,    1,    2,    1,    3,
    2,    1,    3,    1,    2,    2,    3,    1,    1,    1,
    2,    1,    1,    4,    1,    1,    1,    1,    1,    1,
    1,    1,    1,    1,    1,    1,    4,    1,    1,    4,
    4,    2,    1,    3,    1,    1,    2,    3,    2,    1,
    1,    1,    1,    2,    1,    1,    1,    1,    1,    1,
    1,    1,    1,    1,    1,    1,    1,    1,    1,    1,
    1,    1,    1,    1,    1,    1,    1,    1,    1,    1,
    1,    1,    1,    1,    1,    1,    6,    9,   10,    8,
    6,    6,    3,    3,    3,    5,    6,    1,    1,    1,
    1,    1,    1,    1,    1,    1,    1,    2,    2,    1,
    2,    1,    3,    2,    1,    2,    1,    3,    1,    1,
    3,    1,    2,    3,    3,    1,    2,    3,    1,    2,
    1,    2,    1,    2,    3,    4,    1,    3,    2,    1,
    3,    2,    3,    3,    2,    4,    5,    1,    1,    1,
    1,    6,    9,   10,    6,    6,    1,    3,    1,    1,
    2,    2,    2,    1,    3,    5,    1,    2,    3,    1,
    2,    1,    1,    1,    1,    1,    1,    5,    1,    3,
    2,    7,    2,    2,    7,    1,    1,    8,    9,    9,
   10,    0,    1,    5,    6,   11,    5,    7,    5,    5,
    6,    4,    4,    5,    5,    6,    6,    7,    5,    5,
    6,    6,    7,    6,    7,    5,    6,    7,    7,    8,
    6,    4,    4,    6,    7,    6,    2,    6,    4,    4,
    4,    4,    6,    6,    7,    8,    7,    6,    6,    6,
    4,    3,    4,    7,    8,    8,    9,   10,    5,    6,
    5,    5,    6,    3,    4,    5,    6,    8,    4,    5,
    6,    6,    4,    5,    7,    8,    5,    6,   11,    4,
    5,    4,    5,    5,    4,
  };
   static readonly short [] yyDefRed = {            0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    2,    9,   10,   11,    0,    0,    0,    0,    0,    0,
    0,    0,  108,  119,  111,  112,  113,  114,  110,    0,
  149,   51,   52,   53,   54,   55,   56,   57,  334,  193,
  194,  195,    0,   50,    0,  182,  183,  191,  190,  192,
    0,  211,  212,    0,  188,  189,    0,  335,  336,  185,
  186,  187,    0,    0,    0,  109,    0,    0,    0,    0,
    0,  151,  150,    0,    0,    0,    3,    0,    0,    0,
  180,    0,    0,    4,    0,    0,  205,  206,   47,   48,
   58,   49,   59,    0,    0,    0,    0,    0,    0,    0,
    0,  210,    0,    0,    0,    0,    0,    0,  120,    0,
    0,    0,  202,    0,  102,    0,    0,    0,    0,    0,
    0,  155,    0,    0,    0,    0,  198,    0,    0,    0,
   77,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,  181,    5,    6,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,  209,    0,    8,
    0,    7,    0,    0,    0,    0,    0,    0,    0,    0,
  203,    0,  103,    0,    0,    0,    0,  154,    0,  129,
  115,    0,    0,  126,    0,    0,    0,   78,    0,  178,
  179,  171,    0,    0,  172,  215,    0,    0,    0,  213,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
  261,  262,  260,  263,  264,  265,  259,  242,  246,  267,
  266,    0,    0,    0,    0,    0,    0,    0,    0,    0,
  245,  243,  244,    0,    0,    0,    0,    0,    0,    0,
    0,  208,    0,    0,    0,    0,   60,    0,    0,    0,
   86,   85,   14,    0,    0,   79,   84,    0,  201,  197,
  200,  184,    0,    0,    0,    0,    0,    0,  116,    0,
    0,    0,    0,  100,   99,   91,   89,   90,   92,   93,
   94,   95,   13,    0,   87,  175,    0,  170,    0,    0,
    0,    0,    0,    0,    0,  140,  214,    0,    0,    0,
    0,  160,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,  272,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,   16,    0,    0,    0,   80,   15,    0,
  269,  125,  204,  121,  104,  122,  118,  127,    0,    0,
   12,   88,  177,  173,    0,    0,  135,    0,    0,    0,
    0,    0,    0,    0,    0,  346,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,  282,  286,    0,    0,  291,    0,
  339,  347,    0,  142,  156,    0,  161,    0,    0,  165,
    0,  162,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,  255,    0,    0,    0,  253,  254,    0,
    0,    0,    0,    0,    0,    0,    0,   73,   76,    0,
   71,    0,   62,   74,    0,   68,   70,   75,   72,   63,
   64,   61,   18,   17,   83,   82,   81,   96,  320,    0,
  319,    0,  317,  137,    0,    0,    0,    0,  344,    0,
    0,  341,    0,    0,    0,    0,  343,  332,  333,    0,
    0,  330,  353,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,  337,  387,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,  216,  217,  218,  219,  220,
  221,  222,  223,  224,  225,    0,    0,    0,    0,    0,
    0,    0,    0,    0,  130,  283,    0,  292,    0,    0,
    0,  141,  163,  166,   45,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,  273,    0,    0,   21,
    0,    0,    0,    0,    0,    0,    0,    0,  274,    0,
  323,  321,  322,  101,    0,  136,  284,    0,  340,  285,
  268,    0,    0,  297,    0,    0,    0,    0,    0,    0,
  331,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,  226,  227,  238,  239,  240,  241,
  229,  231,  232,  233,  234,  230,  228,  236,  237,  235,
    0,    0,    0,  324,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,  402,    0,    0,  131,   26,
    0,   40,    0,    0,    0,    0,    0,    0,    0,    0,
    0,  256,    0,   23,    0,    0,    0,    0,   37,    0,
   66,    0,   69,  318,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,  382,    0,    0,  279,
  280,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,  403,    0,    0,
    0,    0,    0,    0,  252,  247,  251,  257,    0,    0,
   31,    0,    0,   65,    0,    0,    0,  299,    0,    0,
  300,    0,    0,    0,    0,  354,    0,    0,  427,    0,
    0,  412,    0,    0,  431,    0,  416,    0,  433,  424,
  420,    0,    0,  409,    0,  360,  359,  411,  434,    0,
    0,    0,    0,  357,    0,    0,    0,    0,  258,  271,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,  325,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,  275,    0,  277,    0,    0,    0,    0,    0,  327,
    0,    0,  302,    0,  298,    0,    0,    0,    0,    0,
  355,  384,  428,  393,  413,  388,  417,  386,  421,  410,
  361,  399,  422,  281,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,  398,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,  276,  250,    0,   39,  342,
    0,  345,  328,    0,  309,  310,  311,    0,    0,    0,
    0,  308,  304,  303,  301,    0,    0,    0,    0,  358,
    0,    0,    0,    0,  404,    0,  425,    0,    0,    0,
    0,    0,  385,  326,    0,  338,    0,    0,    0,    0,
    0,   28,    0,  248,  278,  329,  306,    0,    0,    0,
    0,    0,  348,    0,    0,    0,  406,    0,    0,  405,
  426,    0,    0,    0,  418,    0,  249,  307,    0,    0,
    0,    0,    0,  350,    0,  349,    0,  407,    0,    0,
    0,    0,    0,    0,    0,  351,  408,    0,    0,    0,
    0,    0,    0,    0,  356,  429,    0,    0,  316,  312,
  315,    0,    0,    0,    0,    0,  313,  314,
  };
  protected static readonly short [] yyDgoto  = {             9,
   10,   11,   66,   12,   13,   14,  284,  254,  246,   67,
   96,  255,  624,   97,  300,   99,   75,  100,  247,  463,
  231,  482,  465,  466,  467,  468,  256,  922,  285,  117,
  118,  183,  124,  102,  184,   15,  135,  198,  414,  301,
  296,   81,   71,   72,   83,   73,   16,  302,  194,  195,
  172,  103,  200,  566,  701,  232,  233,  923,  318,  890,
  492,  790,  924,  781,  782,  415,  416,  417,  418,  419,
  420,  625,  749,  850,  851, 1003,  483,  703,  704,  929,
  930,  501,  502,  538,  708,  421,  422,  504,
  };
  protected static readonly short [] yySindex = {          359,
   27, -175,   72,   76,  109, 3555, -116, -123,    0,  359,
    0,    0,    0,    0,  -45, 1107,  -17,  171,  186, 2576,
   -3,   -2,    0,    0,    0,    0,    0,    0,    0, -107,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,  244,    0,  249,    0,    0,    0,    0,    0,
  254,    0,    0,   46,    0,    0,  261,    0,    0,    0,
    0,    0, 5870,  -73,   71,    0, -119, -107,  289, 5973,
 4242,    0,    0,   56,   68,  286,    0,  326, 4542,    4,
    0, 4542, 4542,    0,   93,  125,    0,    0,    0,    0,
    0,    0,    0,  350,  148, 5973,  148,  -60, -110, -110,
   78,    0, -107,  -19,  357,   -1,  281,  368,    0,  159,
 5973, 5973,    0,  134,    0, -107,   97,  289,  166, 5973,
  188,    0, -216,  396, 5527,  289,    0,   26,  289, 4542,
    0,  182,  337, 4814,  -86,   16, 4542,  326,   19, 4542,
   20,    0,    0,    0,  207, 5973,  -60,  -60, 2129, 5973,
  -60,  -60, 5973, 5973,  -60, 5973,  -60,    0,  167,    0,
  353,    0, -189,  438,  369, 5785,  240,  464,  -34,  -28,
    0,   79,    0, 5973, 5973,  129, 5973,    0,  126,    0,
    0, -107,  123,    0,  289, 5973,  289,    0,  692,    0,
    0,    0, 6129,  187,    0,    0,  184,  -85, -187,    0,
  326,   42,  -86,  326,   44,  326,  469,  700, 5973, 5973,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,  -31,  471,  477,  483,  484, 6015, 6023, 6015,  481,
    0,    0,    0,  -38, 5973, 5973, 2129, 2129, 5973, 2129,
 5973,    0,  470,  472,  473,  214,    0, -189, 5808,    0,
    0,    0,    0,   -4, 2129,    0,    0,  488,    0,    0,
    0,    0,  266, -107,  -44,  485,  -63,  288,    0, 4967,
  289,  482,  506,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0, 1077,    0,    0, 2688,    0, 4666, -162,
 6004,  -84,  302, 6015,  307,    0,    0,  -86,  326,  184,
  184,    0,  -86,  326,  -86,    0,  189,  525, 4067, -107,
  -18, 5973, 6015, 6015, 6015, 6015,    0,  161, 5878,  190,
   85,  191,  196,  -29, -107,  527,  536, 2129,  541, 2129,
 5692, 5726, 1812,    0, -189,  227,    7,    0,    0, 5844,
    0,    0,    0,    0,    0,    0,    0,    0,  331, 5759,
    0,    0,    0,    0,  332,  329,    0,  535,  533,  547,
 6015,  -40, 6015, 3972, 6015,    0, 5342,  185, 5342,  185,
 5342,  185, 5973, 3764,  185, 5973, 5973, 5342, 4033, 4514,
 5973, 5973, 5973, 6015, 6015, 6015, 6015, 6015, 5973,  -23,
 3881,  273, -222, 1918, 6015, 6015, 6015, 6015, 6015, 6015,
 6015, 6015, 6015, 6015, 6015, 6015, 1545,  185, 5973,  185,
 3972,  199, 5973, 4646,    0,    0, 8938, -116,    0, -116,
    0,    0, 6004,    0,    0,  298,    0,  -86,  184,    0,
  -86,    0,  351, -186,  232,  570,  575, 5973,    3,  239,
  247,  248,  250,    0, 6015, 2129,  180,    0,    0,  373,
  385,  258,  262,  265,  604, -171,  607,    0,    0,  612,
    0,  852,    0,    0,  534,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0, -161,
    0,  238,    0,    0,  298, 8938, 9437, 8938,    0,  387,
 4450,    0,  617, 3915, 4542, 4542,    0,    0,    0, 2129,
 5342,    0,    0, 5973, 2129, 5342, 5973, 2129, 5342, 5973,
 2129, 5973, 2129, 5973, 2129, 2129, 2129, 5342, 5973, 2129,
 5973, 2129, 2129, 2129, 2129,  618,  620,  621,  635,  638,
    5, 5973, 4478,    8, 6015,  643,    0,    0, 5973, 5973,
 5973, 3836,    9,  301,  309,  310,  311,  312,  314,  317,
  321,  323,  324,  327,  330,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0, 5973, 1484,  -62, 5973,  338,
 5973, 4542, 3922, -245,    0,    0, -116,    0,   68,   68,
 4773,    0,    0,    0,    0,  437,  446,  462, -167, 5973,
   10, 6015, 5973, 5973, 5973, 5973,    0,  659, -116,    0,
  465,  468,  474,  346,  479,  475,  347, 5096,    0, 5726,
    0,    0,    0,    0, 5759,    0,    0, -116,    0,    0,
    0,  685,  490,    0,  690, 3915, 4542, 3915,  693, 2129,
    0, 2129,  699, 2129, 2129,  702, 2129, 2129,  707, 2129,
  710, 2129,  713,  714,  715, 2129, 2129,  718, 2129,  719,
  720,  722,  723, 6015, 6015, 6015, 1812, 6015, 1195,   17,
 5973,   22, 5973,  725, 5973, 2129, 2129,   25, 5973,   28,
 6015, 5973, 5973, 5973, 5973, 5973, 5973, 5973, 5973, 5973,
 5973, 5973, 5973, 2129,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
 5973, 4450,  728,    0, 2129,  345,  690,  690, 3915, 3915,
 5973, 5973,  338, 5973, 4542,    0, 6015,   68,    0,    0,
 -116,    0,  516,  520,   30, 6015,  742,  -25,  -24,  -22,
  -20,    0,   68,    0, -116, -116,  532,  746,    0,  542,
    0,  279,    0,    0,   68,  490,  701, 5659,  408,  690,
 3915,  690, 4450,  756,  758, 4450,  760,  766, 4450,  769,
  771, 4450,  774, 4450,  775, 4450, 4450, 4450,  776,  778,
 4450,  781, 4450, 4450, 4450, 4450,    0,  782,  783,    0,
    0,  784,  785,  548,  786, 5973,   31, 5973, 2129,  787,
 5973,  788,  790,  800, 6015,   35, 6015,  801, -107, -107,
 -107, -107, -107, -107, -107, -107, -107, -107, -107, -107,
  804, 2129,  807,  764,  808,  597,  184,  184,  690,  690,
 3915, 3915,  690,  690, 3915, 3915, 4542,    0,   68,  814,
 -116, 6015,  815,  985,    0,    0,    0,    0,   68,   68,
    0,  487, -116,    0,  817, 5973, 5913,    0, 3645,  283,
    0,  490,  476,  690,  493,    0, 4450, 4450,    0, 4450,
 4450,    0, 4450, 4450,    0, 4450,    0, 4450,    0,    0,
    0, 4450, 4450,    0, 4450,    0,    0,    0,    0, 6015,
 6015, 1812, 1812,    0,  494,  819, 5973,  823,    0,    0,
  495,  825,  496, 5973, 5973,  830, 6015,  834,  985, 4450,
  840, 4450,    0, 6015,  841,  184,  184,  184,  184,  690,
  690,  184,  184,  690,  690, 3915,  505,   68,  843,  985,
 6015,    0,  294,    0,  633,   68,  490,  847, 5937,    0,
  824, 2380,    0, 3829,    0, 5965,  563,  490,  503,  490,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,  784,  637,  514,  -42,  515,  642,
  519,  644, 2129, 2129,  985,  858,  985,  859,    0, 4450,
  811,  864,  652,  184,  184,  184,  184,  184,  184,  184,
  184,  690,  658,  985,  313,    0,    0,  985,    0,    0,
  490,    0,    0, 5633,    0,    0,    0,  539,  876,  886,
  887,    0,    0,    0,    0,  490,  593,  490,  605,    0,
  676,  893,  558,  687,    0,  688,    0,  615,  619,  859,
  985,  859,    0,    0, 6015,    0,  184,  184,  184,  184,
  184,    0,  382,    0,    0,    0,    0,  384,  -13, 6015,
 6015, 6015,    0,  490,  622,  490,    0,  573,  698,    0,
    0,  912,  916,  859,    0,  184,    0,    0,  924, 5973,
  576,  577,  579,    0,  490,    0,  724,    0,  594,  595,
 5973,   36, 5973, 5973, 5973,    0,    0,  726,  727,   37,
 6015,  -15,   -5,    2,    0,    0, 6015,  945,    0,    0,
    0,  946,  985,  985,  398,  413,    0,    0,
  };
  protected static readonly short [] yyRindex = {            0,
    0,    0,    0,    0,    0,    0, 4109,    0,    0,  992,
    0,    0,    0,    0,    0,    0,    0,    0,    0,  678,
    0,    0,    0,    0,    0,    0,    0,    0,    0, 1241,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0, 1649,    0,    0,    0,    0,    0,
 1849,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0, 4378,  820,  721,    0,
    0,    0,    0,    0, 4159,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,  678,    0,  678,    0,  678,  678,
    0,    0,  162,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,  209,    0,    0,    0,    0,
    0,    0, 4461,    0,    0,  733,    0,    0,  735,    0,
    0,    0,    0,    0,  678,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,  139,    0,
    0,    0,    0,    0,    0,    0,    0,    0,  645,    0,
    0,    0,    0,    0,    0,    0,    0,    0,  139,  139,
    0,    0,    0,    0,    0,    0,    0,    0, 1445,    0,
    0,  433,    0,    0,  743,    0,  744,    0,    0,    0,
    0,    0,  578,    0,    0,    0,  -77,    0,  -75,    0,
    0,    0,  581,    0,    0,    0,    0,  761,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,  480,    0,    0,  139,  139,    0,  139,
    0,    0,    0,    0,    0,    0,    0,    0,    0, 1975,
    0,    0,    0,    0,  139,    0,    0,    0,    0,    0,
    0,    0,    0,  297,  139,    0,  139,    0,    0,    0,
  745, 1326, 2526,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,  441,    0,    0,  -65,
    0,    0,    0,    0,    0,    0,    0,  678,    0,  882,
 1037,    0,  873,    0,  678, 1731,    0, 1104,  139,  656,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,  938, 1004,    0,    0,  139, 1171,  139,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0, 6050,    0, 6050,
    0, 6050,    0,    0, 6050,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0, 1890,    0, 6050,
    0,    0,    0,    0,    0,    0,    0, 4927,    0, 9043,
    0,    0,    0,    0,    0,  -61,    0,  678, 1185,    0,
  678,    0,    0,    0,    0,    0,    0,    0,  139,    0,
    0,    0,    0,    0,    0,  209,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0, 1591,    0,    0,  101,
    0,  139,    0,    0,  450,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,  139,
    0,    0,    0,    0,  -56,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,  139,
    0,    0,    0,    0,  139,    0,    0,  139,    0,    0,
  139,    0,  139,    0,  139,  139,  139,    0,    0,  139,
    0,  139,  139,  139,  139,    0,    0,    0,    0,    0,
  139,    0,    0,  139,    0,    0,    0,    0,    0,    0,
    0,    0,  139,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,  139,    0,    0,
    0,    0,    0,  139,    0,    0, 5052,    0, 5185, 9148,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
  139,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0, 9253,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,  139,
    0,  139,    0,  139,  139,    0,  139,  139,    0,  139,
    0,  139,    0,    0,    0,  139,  139,    0,  139,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,  139,
    0,  139,    0,    0,    0,  139,  139,  139,    0,  139,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,  139,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0, 6132,    0,  139,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0, 5310,    0,    0,
 1723,    0,    0,    0,  139,    0,    0,  139,  139,  139,
  139,    0, 1790,    0,    0,    0,    0, 1915,    0,    0,
    0,    0,    0,    0, 9358,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0, 6241,    0,    0,    0,    0,  139,    0,  139,    0,
    0,    0,    0,    0,    0,  139,    0,    0, 2083, 2191,
 2333, 2441, 2549, 2691, 2799, 2907, 3049, 3157, 3265, 3407,
    0,  139,    0,    0,    0,    0, 6349,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0, 1924, 1944,
 1985,    0,    0,    0,    0,    0,    0,    0, 2226, 2336,
    0,    0, 2694,    0,    0,    0,    0,    0,  139,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0, 6457, 6565, 6673, 6781,    0,
    0, 6889,    0,    0,    0,    0,    0, 3052,    0,    0,
    0,    0,    0,    0,    0, 3708,    0,    0,    0,    0,
  455,  139,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0, 6997,    0,    0,    0,    0,    0,
    0,    0,  139,  139,    0,    0,    0, 7105,    0,    0,
    0,    0,    0, 7213, 7321, 7429,    0, 7537, 7645, 7753,
 7861,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0, 7969,
    0, 8077,    0,    0,    0,    0, 8185, 8293, 8401, 8509,
 8617,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0, 8725,    0, 8833,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,  139,    0,    0,    0,    0,    0,    0,    0,  139,
    0,  139,  139,  139,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,
  };
  protected static readonly short [] yyGindex = {            0,
    0,  986,  894,    0,    0,    0,    0,  777,  770, 1007,
 3213,   -6,  486,    0,   77,   64, -153,    0,  696,  703,
 -308, -603,    0,  418,    0, -776,    0,  153,  748,  918,
  610,    0,  145,    0,  772,    0, -103,    0,  616, -134,
 -270,    6,    0,  -12,  -68,  973,    0, -220,    0,  752,
    0,    0, -191,    0,    0,    0,    0,  400, -121,    0,
 -595, -631,   55,  165,  170, -381, -295,    0,  631,  632,
  568,   84, 3825,    0,  120,    0,  443,    0,  245,    0,
  131,  -74,  135,    0,  348,    0,  583,  111,
  };
  protected static readonly short [] yyTable = {            68,
  199, 1013,  130,   82,  742,  323,  260,  297,  312,   68,
  137,   70,  261,  140,  452,  835,  836,  344,  837,  357,
  838,  438,  464,  464,  471, 1089, 1060,  747,  702,  346,
  107,  165,  576,  792,  203, 1090,   64,  291,  423,  340,
  161,  481, 1091,  125,  292,  132,  592,  138,  659,  120,
  340,  663,  671,  726,  122,  125,  116,  133,  125,  125,
  786,  139,  290,   68,   68,  788,  134,   65,  795,  928,
  716,  797,   68,  832,  887,   68,   68,  427,  897, 1081,
 1087,  125,  430,  125,  432,   64,  243,   17,  142,  149,
   39,  244,  293,  586,   18,   19,   98,  298,  297,   63,
  303,  611,  305,   63,  169,  170,  320,  321,  605,  297,
  178,  612,  723,  116,  613,  186,   65,  355,  182,  262,
  339,   68,  263,   68,  287,  539,  294,  193,  445,  540,
   68,  474,   20,   68,  128,  142,   21,  196,  108,  208,
  174,   72,  142,  234,   72,  142,  237,  238,   63,  240,
  845,  294,  928,  609,  886,  582,  888,   74,  148,  892,
  152,  124,  196,  269,  101,  429,  270,  264,  265,   22,
  267,  147,  174,  151,  109,  155,  157,  449,  107,   68,
  124,   87,   88,  119,  717,   89,   90,   39,   74,  132,
  617,  245,  620,  295,  587,  428,  132,  447,  138,  576,
  431,   76,  309,  310,  445,   87,   88,  583,  133,  606,
  584,  197,  139,  724,  616,   52,   53,  134,  356,   58,
   59,  173,  108,  445,   72,  490,   78,  288,  324,  325,
  289,   85,  328,  445,  330,   89,   90,  297,  196,  101,
   23,  101,   84,  101,  101,  491,   86,  159,  107,   24,
  124,  448,  105,  266,  334,  958,  937,  335,   25,   26,
   27,   28,   29,  182,  579,   30,  580,  473,  160,  338,
  335,  105,  164,   94,  106,  138,  108,  104,  614,  101,
  338,  615,  193,  110,  124,  444,  532,  201,  111,   23,
  204,  206,  142,  112,  506,  496,  509,   94,   24,  481,
  114,  464,  113,  518,  598,  439,  481,   25,   26,   27,
   28,   29,  446,  299,   30,  304,   58,   59,  257,  844,
  108,  108,  615,  935,  462,  462,  936,  121,  125,  131,
  437,  990,   39,  105,  987, 1059,  107,  988,  124,  108,
  106,  132, 1007,  480, 1009,  108,  133,  101,  780,  108,
  311,  495,  143, 1034,  108,  108,  988,   68,  108,  108,
  500,  108,  505,  108,  508,  134,  511,  513,  108,  515,
  516,  517,  520,  522,  523,  524,  525,  125,  108,  317,
  317,  317,  531,  534,  144,  108,  108,  543,  108,  145,
 1038,  108,  108,  108,  158, 1036,  163,  228,  572, 1012,
  108,  257,  568,  166,   68,  108,  574,  167,  108,  171,
 1043,  108, 1045,  108,  108,  168,  571,  533,  108,  108,
  108,  106, 1057,  718, 1058,  988,  627,  615,  229,  124,
  124,  591,  175,  124,  124,  124,  124,  179, 1097,   87,
   88,  988,  101,   89,   90,  733,  425,  101, 1064,  101,
 1066,  124,  124, 1098,  177,  188,  988,  124,  124,  189,
  227,   58,   59,  207,  745,  440,  441,  442,  443, 1076,
   93,  317,  107,  128,  124,  120,  128,  248,  124,   20,
  507,  176,  510,  242,  176,  514,  124,  124,   68,   68,
   67,  249,  477,   67,  630,  305,  258,  632,  305,  634,
  635,  142,  637,  638,  259,  640,  268,  642,  196,  306,
  313,  646,  647,  489,  649,  493,  314,  497,  567,  107,
  569,  124,  315,  316,  322,  660,  662,  331,  342,  332,
  333,   94,  666,  667,  668,  670,  526,  527,  528,  529,
  530,  343,  349,  536,  347,  350,  345,  544,  545,  546,
  547,  548,  549,  550,  551,  552,  553,  554,  555,  684,
  715,  424,  705,  426,   68,   68,   68,  829,  434,  433,
  453,  450,  101,  780,  780,  101,  451,  711,  714,  454,
  159,  839,  840,  725,  456,  485,  728,  729,  730,  731,
  478,  484,  486,  487,  211,  212,  213,  597,  214,  215,
  216,  480,  217,  462,  488,  503,  537,  585,  480,  218,
  219,  294,  588,  589,  590,   69,  220,  107,  174,  124,
   68,  174,  573, 1002,  221,   80,    1,    2,  593,  599,
    3,    4,  142,    5,  230,  631,  594,  595,  601,  596,
  631,  600,  602,  631,  207,  603,  827,  604,    6,    7,
  607,  608,  631,  707,  787,   25,  789,  610,  789,  621,
  623,  654,  796,  655,  656,  799,  800,  801,  802,  803,
  804,  805,  806,  807,  808,  809,  810,  918,  657,  126,
  129,  658,  906,  907,    8,  481,  665,  664,  136,  926,
  672,  139,  141,  308,  812,  107,  720,  124,  673,  674,
  675,  676,  721,  677,   68,   68,  678,   68,   68,  750,
  679,  752,  680,  681,  297,  297,  682,  222,  722,  683,
  732,  734,  326,  327,  735,  329,  737,  740,  746,  748,
  736,  739,  223,  224,  225,  226,  753,  185,  738,  187,
  341,  849,  756,  307,  727,  759,  202,   20,   20,  205,
  762,   20,   20,  764,   20,  490,  766,  767,  768,  228,
   46,  771,  773,  774,  706,  775,  776,  207,  791,   20,
   20,  814,  816,  974,  975,  830,  831,  978,  979,  789,
  934,  789,  297,  297,  789,  834,  297,  297,  841,  842,
  229,  846,  819,  820,  436,  271,  823,  852,  843,  857,
  107,  858,  124,  860,  884,   20,  777,  778,  779,  861,
  783,  785,  863,  455,  864,  457,  283,  866,  868,  872,
   68,  873,  227,  798,  875,  880,  881,  882,  883,  885,
  891,  893,  142,  894,  854,  297,  297,  297,  297,  462,
  932, 1027, 1028,  895,  899, 1029, 1030,  900,  159,  159,
  902,  904,  159,  159,  702,  159,  905,  917,  920,  107,
  927,  124,  957,  994,  297,  938,  959,  925,  961,  828,
  159,  159,  164,  965,  956,  960,  962,  967,  833,  107,
  789,  167,  940,  970,  973,  983,  984,  963,  964,  989,
  991, 1006, 1008, 1010, 1011, 1014, 1056,  124, 1015, 1016,
 1017, 1021,  988, 1024,  910,  911,  159, 1025,  914,  915,
  107, 1026,  207,  207, 1032, 1040,  207,  207,  207,  207,
 1039, 1044,  462,   25,   25, 1041, 1042,   25,   25,  849,
   25,  341, 1047, 1046,  207,  207, 1048,   22, 1049,  142,
  207,  207,  107, 1050, 1051,   25,   25,  896, 1052,  898,
 1065,  272, 1053, 1067, 1068, 1069,  211,  212,  213, 1070,
  214,  215,  216, 1071,  217, 1073, 1074,  273, 1075,  207,
  207,  218,  219,  494, 1078, 1079,  622,  107,  220,  124,
 1077,   25, 1085, 1086,  919,  629,  221,  480, 1093, 1094,
  633,    1,  143,  636,  124,   77,  639,  162,  641,  982,
  643,  644,  645,   24,  144,  648,  145,  650,  651,  652,
  653,  274,  275,  276,  146,  148,  147,  336,  277,  278,
  570,  279,  280,  281,  282,  337,   95,  743,   46,   46,
  472,  352,  952,  953,  470,   46,  169,  176,  581,  123,
  354,  348, 1035,  107,   64,  124,  954,  577,  578,  966,
   46,   46,  955, 1072,  618, 1005,  972,  744,  903,  993,
  824,    0,    0,    0, 1080,    0, 1082, 1083, 1084,  619,
    0,    0,    0,  986,    0,   65,  107,  107,  107,  222,
  107,  107,  107,  108,  107,    0,   46,    0,    0,    0,
    0,  107,  107,    0,  223,  224,  225,  226,  107,    0,
    0,    0,    0,   19,  626,  628,  107,   63,  211,  212,
  213,    0,  214,  215,  216,  754,  217,  755,    0,  757,
  758,    0,  760,  761,    0,  763,    0,  765,    0,    0,
  220,  769,  770,    0,  772,    0,    0,    0,  221,    0,
  164,  164,    0,    0,  164,  164,    0,  164,    0,  167,
  167,  793,  794,  167,  167,    0,  167,    0,    0,    0,
    0,    0,  164,  164,    0,    0,   64,    0,    0,  811,
   36,  167,  167,    0,    0,    0,    0, 1055,    0,    0,
  709,  710,  713,    0,  168,    0,    0,  813,    0,  124,
  815,    0, 1061, 1062, 1063,    0,    0,   65,  164,  107,
    0,  351,    0,    0,    0,   22,   22,  167,    0,   22,
   22,    0,   22,    0,  107,  107,  107,  107,    0,    0,
    0,    0,    0,    0,    0,    0,    0,   22,   22,   63,
    0,    0,    0, 1088,    0,  108,  751,    0,  856, 1092,
  124,  859,    0,    0,  862,    0,  107,  865,  250,  867,
    0,  869,  870,  871,   64,    0,  874,   24,  876,  877,
  878,  879,    0,   22,    0,    0,   25,   26,   27,   28,
   29,   24,   24,   30,  889,   24,   24,    0,   24,    0,
  124,  124,  124,    0,  124,   65,    0,    0,    0,    0,
    0,    0,    0,   24,   24,    0,    0,  901,  968,    0,
  124,    0,  124,    0,  169,  169,    0,    0,  169,  169,
    0,  169,    0,    0,    0,    0,    0,   63,    0,  985,
  821,  822,    0,  825,  826,    0,  169,  169,    0,   24,
    0,  124,    0,  124,  933,    0,  272,    0,    0,    0,
    0,    0,  941,  942,    0,  943,  944,    0,  945,  946,
    0,  947,  273,  948,    0,    0,    0,  949,  950,    0,
  951,    0,  169,  124, 1020,  124, 1022,  921,    0,    0,
   23,   19,   19,    0,    0,   19,   19,    0,   19,   24,
    0,    0,    0, 1033,    0,  969,    0,  971,   25,   26,
   27,   28,   29,   19,   19,   30,  274,  275,  276,    0,
   79,    0,    0,  277,  278,    0,  279,  280,  281,  282,
    0,    0,    0,   39,   40,   41,    0,   42,   43, 1004,
 1054,   45,    0,    0,   46,   47,   48,   49,   50,   19,
   51,    0,    0,    0,    0,    0,  916,    0,   36,   36,
    0,    0,   36,   36,  117,   36,    0,    0, 1018, 1019,
   97,    0,  168,  168,    0, 1023,  168,  168,  250,  168,
   36,   36,    0,    0,    0,    0,    0,   24,    0,    0,
    0,    0,    0,    0,  168,  168,   25,   26,   27,   28,
   29,    0,    0,   30,  117,  117,  117,   54,  117,    0,
    0,    0, 1095, 1096,    0,    0,   36,  124,  124,  124,
    0,  124,  124,  124,  117,  124,  117,    0,  124,  124,
  168,    0,  124,  124,  124,  124,  124,    0,    0,  124,
    0,    0,    0,    0,    0,    0,    0,  124,    0,    0,
  124,  124,    0,    0,  124,  117,    0,  117,    0,   55,
   56,   57,   58,   59,   60,   61,   62,    0,  124,  124,
    0,  124,  124,    0,    0,  124,  124,    0,  124,  124,
  124,  124,  124,    0,  124,    0,  124,  117,    0,  117,
    0,    0,    0,    0,    0,  784,    0,  124,  124,  124,
    0,  124,  124,    0,    0,   97,  124,    0,  124,    0,
   34,  124,  124,  124,  124,  124,  124,  124,  124,  124,
  124,   97,  124,  124,    0,  124,  124,  124,  124,  124,
  124,  124,  124,  124,  124,  124,  124,  124,    0,  124,
  124,  124,    0,    0,    0,  124,  124,  124,  124,  124,
    0,  124,  124,  124,  124,  124,  124,  124,  124,  124,
    0,    0,    0,    0,    0,   97,   97,   97,    0,    0,
  124,    0,   97,   97,    0,   97,   97,   97,   97,    0,
    0,    0,  124,  124,  124,  124,    0,  124,    0,  124,
  124,    0,    0,  124,  124,  124,    0,    0,  124,  124,
  124,    0,    0,    0,    0,    0,    0,    0,    0,  196,
    0,    0,  196,    0,    0,    0,    0,    0,    0,    0,
    0,  117,  117,  117,    0,  117,  117,  117,  196,  117,
    0,    0,  117,  117,    0,    0,  117,  117,  117,  117,
  117,    0,   43,  117,    0,    0,    0,    0,    0,    0,
  123,  117,    0,    0,  117,  117,    0,    0,  117,  196,
    0,    0,    0,    0,  685,  686,    0,    0,    0,    0,
    0,    0,  117,  117,    0,  117,  117,    0,    0,  117,
  117,    0,  117,  117,  117,  117,  117,    0,  117,    0,
  117,  196,    0,    0,    0,    0,    0,    0,    0,    0,
    0,  117,  117,  117,    0,  117,  117,    0,    0,   29,
  117,    0,  117,    0,    0,  117,  117,  117,  117,  117,
  117,  117,  117,  117,  117,    0,  117,  117,    0,  117,
  117,  117,  117,  117,  117,  117,  117,  117,  117,  117,
  117,  117,    0,  117,  117,    0,    0,    0,  117,  117,
  117,  117,  117,  117,    0,  117,  117,  117,  117,  117,
  117,  117,  117,  117,    0,    0,    0,    0,    0,    0,
    0,    0,    0,  123,  117,    0,    0,    0,   34,   34,
    0,    0,   34,   34,    0,   34,  117,  117,  117,  117,
    0,  117,    0,  117,  117,    0,    0,  117,  117,  117,
   34,   34,  117,  117,  117,  687,  688,  689,  690,  199,
    0,    0,  199,    0,  691,  692,  693,  694,  695,  696,
  697,  698,  699,  700,    0,  196,  196,  196,  199,  196,
  196,  196,  196,  196,   38,    0,   34,    0,    0,    0,
  196,  196,    0,   44,    0,    0,    0,  196,    0,    0,
  196,  196,  196,  196,  196,  196,    0,  196,    0,  199,
    0,    0,  196,   27,  556,  557,  558,  559,  560,  561,
  562,  563,  564,  565,    0,    0,  196,  196,    0,  196,
  196,    0,    0,  196,    0,    0,  196,  196,  196,  196,
  196,  199,  196,    0,    0,    0,    0,   64,    0,    0,
    0,    0,    0,    0,   41,    0,    0,    0,    0,    0,
   43,   43,    0,    0,   43,   43,    0,   43,  123,  123,
    0,    0,  123,  123,  123,  123,    0,    0,   65,    0,
    0,    0,   43,   43,  108,  270,    0,    0,  270,    0,
  123,  123,    0,    0,    0,    0,  123,  123,  196,  196,
    0,    0,    0,    0,    0,    0,  270,    0,    0,    0,
   63,    0,    0,  196,  196,  196,  196,  125,   43,    0,
    0,    0,    0,    0,    0,  123,  123,   29,   29,    0,
    0,   29,   29,    0,   29,    0,    0,  270,  211,  212,
  213,    0,  214,  215,  216,    0,  217,    0,    0,   29,
   29,  196,  196,  196,    0,    0,  196,  196,  196,    0,
  220,    0,    0,    0,    0,    0,    0,  270,  221,  270,
    0,    0,    0,    0,    0,  199,  199,  199,    0,  199,
  199,  199,  199,  199,    0,   29,    0,    0,    0,    0,
  199,  199,  107,    0,  124,    0,    0,  199,    0,    0,
  199,  199,  199,  199,  199,  199,    0,  199,    0,    0,
    0,    0,  199,    0,    0,    0,    0,    0,    0,    0,
  352,  352,    0,    0,    0,    0,  199,  199,    0,  199,
  199,    0,    0,  199,    0,    0,  199,  199,  199,  199,
  199,    0,  199,    0,    0,    0,    0,    0,    0,    0,
    0,   23,   38,   38,    0,    0,   38,   38,  228,   38,
   24,   44,   44,    0,    0,   44,   44,    0,   44,   25,
   26,   27,   28,   29,   38,   38,   30,    0,    0,    0,
    0,   27,   27,   44,   44,   27,   27,    0,   27,  229,
    0,    0,    0,    0,    0,   30,    0,    0,  199,  199,
  107,  270,  124,   27,   27,    0,    0,    0,    0,    0,
   38,    0,    0,  199,  199,  199,  199,  270,  270,   44,
  270,  227,   41,   41,    0,    0,   41,   41,    0,   41,
    0,    0,    0,    0,    0,    0,  541,    0,    0,   27,
    0,    0,    0,    0,   41,   41,    0,    0,    0,    0,
    0,  199,  199,  199,    0,    0,  199,  199,  199,    0,
    0,  352,  352,  352,  352,    0,    0,    0,    0,  542,
  352,  352,  352,  352,  352,  352,  352,  352,  352,  352,
   41,  270,  270,  270,    0,  270,  270,    0,    0,    0,
  270,    0,  270,    0,    0,  270,  270,  270,  270,  270,
  270,  270,  270,  270,  270,   32,  270,  270,    0,  270,
  270,  270,  270,  270,  270,  270,  270,  270,  270,  270,
  270,  270,    0,  270,  270,  430,  430,    0,    0,  270,
  270,  270,  270,  270,  270,  270,  270,  270,  270,  270,
  270,  270,  107,  270,  124,    0,    0,    0,    0,    0,
    0,    0,    0,    0,  270,  211,  212,  213,    0,  214,
  215,  216,    0,  217,    0,    0,  270,  270,  270,  270,
  218,  219,    0,  270,    0,    0,    0,  220,    0,    0,
    0,    0,    0,    0,    0,  221,    0,    0,    0,  430,
  430,  430,    0,  430,  430,    0,    0,    0,  430,    0,
  430,    0,    0,  430,  430,  430,  430,  430,  430,  430,
  430,  430,  430,    0,  430,  430,    0,  430,  430,  430,
  430,  430,  430,  430,  430,  430,  430,  430,  430,  430,
    0,  430,  430,  435,  435,    0,    0,  430,  430,  430,
  430,  430,    0,  430,  430,  430,  430,  430,  430,  430,
  107,  430,  124,    0,    0,    0,    0,    0,    0,    0,
    0,    0,  430,   30,   30,    0,    0,   30,   30,    0,
   30,    0,    0,    0,  430,  430,  430,  430,  222,    0,
    0,  430,  108,    0,    0,   30,   30,    0,    0,    0,
    0,    0,    0,  223,  224,  225,  226,  435,  435,  435,
    0,  435,  435,    0,    0,    0,  435,    0,  435,    0,
    0,  435,  435,  435,  435,  435,  435,  435,  435,  435,
  435,   30,  435,  435,    0,  435,  435,  435,  435,  435,
  435,  435,  435,  435,  435,  435,  435,  435,    0,  435,
  435,    0,    0,    0,    0,  435,  435,  435,  435,  435,
    0,  435,  435,  435,  435,  435,  435,  435,  107,  435,
  124,    0,    0,    0,    0,    0,    0,    0,    0,    0,
  435,    0,    0,   32,   32,  419,  419,   32,   32,    0,
   32,    0,  435,  435,  435,  435,    0,    0,    0,  435,
    0,    0,    0,    0,    0,   32,   32,    0,    0,    0,
    0,    0,    0,    0,    0,    0,  211,  212,  213,    0,
  214,  215,  216,    0,  217,    0,    0,    0,    0,    0,
   98,  995,  996,    0,    0,  997,    0,    0,  220,    0,
    0,   32,    0,    0,    0,    0,  221,    0,    0,  419,
  419,  419,    0,  419,  419,    0,    0,    0,  419,    0,
  419,    0,    0,  419,  419,  419,  419,  419,  419,  419,
  419,  419,  419,   33,  419,  419,    0,  419,  419,  419,
  419,  419,  419,  419,  419,  419,  419,  419,  419,  419,
    0,  419,  419,  392,  392,    0,    0,  419,  419,  419,
  419,  419,    0,  419,  419,  419,  419,  419,  419,  419,
  107,  419,  124,    0,    0,    0,    0,    0,    0,    0,
    0,    0,  419,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,  419,  419,  419,  419,    0,  998,
    0,  419,    0,  108,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,  999, 1000, 1001,  392,  392,  392,
    0,  392,  392,    0,    0,   98,  392,    0,  392,    0,
    0,  392,  392,  392,  392,  392,  392,  392,  392,  392,
  392,   98,  392,  392,    0,  392,  392,  392,  392,  392,
  392,  392,  392,  392,  392,  392,  392,  392,    0,  392,
  392,  389,  389,    0,    0,  392,  392,  392,  392,  392,
    0,  392,  392,  392,  392,  392,  392,  392,  107,  392,
  124,    0,    0,    0,    0,   98,   98,   98,    0,    0,
  392,    0,   98,   98,    0,   98,   98,   98,   98,    0,
    0,    0,  392,  392,  392,  392,    0,   87,   88,  392,
    0,   89,   90,   91,   32,   92,   33,   34,   35,   36,
   37,   38,    0,    0,    0,  389,  389,  389,   44,  389,
  389,    0,    0,    0,  389,    0,  389,    0,   93,  389,
  389,  389,  389,  389,  389,  389,  389,  389,  389,    0,
  389,  389,    0,  389,  389,  389,  389,  389,  389,  389,
  389,  389,  389,  389,  389,  389,    0,  389,  389,    0,
    0,    0,    0,  389,  389,  389,  389,  389,    0,  389,
  389,  389,  389,  389,  389,  389,  107,  389,  124,    0,
    0,    0,    0,    0,    0,    0,    0,    0,  389,   94,
  353,   33,   33,  390,  390,   33,   33,    0,   33,    0,
  389,  389,  389,  389,    0,    0,    0,  389,    0,    0,
    0,  127,    0,   33,   33,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,   40,   41,    0,   42,   43,
    0,    0,   45,    0,    0,   46,   47,   48,   49,   50,
    0,   51,    0,    0,    0,    0,    0,    0,    0,   33,
    0,    0,    0,    0,    0,    0,    0,  390,  390,  390,
    0,  390,  390,    0,    0,    0,  390,    0,  390,    0,
    0,  390,  390,  390,  390,  390,  390,  390,  390,  390,
  390,   42,  390,  390,    0,  390,  390,  390,  390,  390,
  390,  390,  390,  390,  390,  390,  390,  390,   54,  390,
  390,  391,  391,    0,    0,  390,  390,  390,  390,  390,
    0,  390,  390,  390,  390,  390,  390,  390,  107,  390,
  124,    0,    0,    0,    0,    0,    0,    0,    0,    0,
  390,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,  390,  390,  390,  390,    0,    0,    0,  390,
   55,   56,   57,    0,    0,   60,   61,   62,    0,    0,
    0,    0,    0,    0,    0,  391,  391,  391,    0,  391,
  391,    0,    0,    0,  391,    0,  391,    0,    0,  391,
  391,  391,  391,  391,  391,  391,  391,  391,  391,    0,
  391,  391,    0,  391,  391,  391,  391,  391,  391,  391,
  391,  391,  391,  391,  391,  391,    0,  391,  391,  432,
  432,    0,    0,  391,  391,  391,  391,  391,    0,  391,
  391,  391,  391,  391,  391,  391,  107,  391,  124,    0,
    0,    0,    0,    0,    0,    0,    0,    0,  391,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
  391,  391,  391,  391,    0,    0,    0,  391,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,  432,  432,  432,    0,  432,  432,    0,
    0,    0,  432,    0,  432,    0,    0,  432,  432,  432,
  432,  432,  432,  432,  432,  432,  432,    0,  432,  432,
    0,  432,  432,  432,  432,  432,  432,  432,  432,  432,
  432,  432,  432,  432,    0,  432,  432,    0,    0,    0,
    0,  432,  432,  432,  432,  432,    0,  432,  432,  432,
  432,  432,  432,  432,  107,  432,  124,  146,    0,  150,
  153,  154,  156,    0,    0,    0,  432,    0,    0,   42,
   42,  423,  423,   42,   42,    0,   42,    0,  432,  432,
  432,  432,    0,    0,    0,  432,    0,    0,    0,    0,
    0,   42,   42,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,  209,
  210,    0,    0,  235,  236,    0,    0,  239,    0,  241,
    0,    0,    0,    0,    0,    0,    0,   42,    0,    0,
    0,    0,    0,    0,    0,  423,  423,  423,    0,  423,
  423,    0,    0,    0,  423,    0,  423,    0,    0,  423,
  423,  423,  423,  423,  423,  423,  423,  423,  423,    0,
  423,  423,    0,  423,  423,  423,  423,  423,  423,  423,
  423,  423,  423,  423,  423,  423,    0,  423,  423,  415,
  415,    0,    0,  423,  423,  423,  423,  423,    0,  423,
  423,  423,  423,  423,  423,  423,  107,  423,  124,    0,
    0,    0,    0,    0,    0,    0,    0,    0,  423,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
  423,  423,  423,  423,    0,    0,    0,  423,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,  415,  415,  415,    0,  415,  415,    0,
    0,    0,  415,    0,  415,    0,    0,  415,  415,  415,
  415,  415,  415,  415,  415,  415,  415,    0,  415,  415,
    0,  415,  415,  415,  415,  415,  415,  415,  415,  415,
  415,  415,  415,  415,    0,  415,  415,  401,  401,    0,
    0,  415,  415,  415,  415,  415,    0,  415,  415,  415,
  415,  415,  415,  415,    0,  415,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,  415,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,  415,  415,
  415,  415,    0,    0,    0,  415,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,  401,  401,  401,    0,  401,  401,    0,    0,    0,
  401,    0,  401,    0,   64,  401,  401,  401,  401,  401,
  401,  401,  401,  401,  401,    0,  401,  401,    0,  401,
  401,  401,  401,  401,  401,  401,  401,  401,  401,  401,
  401,  401,    0,  401,  401,   65,    0,    0,    0,  401,
  401,  401,  401,  401,    0,  401,  401,  401,  401,  401,
  401,  401,    0,  401,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,  401,    0,    0,   63,    0,  362,
  362,    0,    0,    0,    0,    0,  401,  401,  401,  401,
    0,    0,    0,  401,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,  228,    0,    0,   35,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,  229,    0,    0,    0,    0,
    0,    0,    0,  362,  362,  362,    0,  362,  362,    0,
    0,    0,  362,    0,  362,    0,    0,  362,  362,  362,
  362,  362,  362,  362,  362,  362,  362,  227,  362,  362,
    0,  362,  362,  362,  362,  362,  362,  362,  362,  362,
  362,  362,  362,  362,    0,  362,  362,    0,    0,    0,
    0,  362,  362,  362,  362,  362,    0,  362,  362,  362,
  362,  362,  362,  362,    0,  362,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,  362,    0,   23,    0,
    0,    0,    0,   64,    0,    0,    0,   24,  362,  362,
  362,  362,    0,    0,    0,  362,   25,   26,   27,   28,
   29,    0,    0,   30,    0,    0,    0,    0,   31,    0,
    0,    0,    0,   32,   65,   33,   34,   35,   36,   37,
   38,   39,   40,   41,    0,   42,   43,   44,    0,   45,
    0,    0,   46,   47,   48,   49,   50,    0,   51,    0,
    0,    0,    0,    0,    0,    0,   63,    0,  228,   52,
   53,    0,    0,    0,    0,   64,    0,    0,    0,    0,
    0,  211,  212,  213,    0,  214,  215,  216,    0,  217,
    0,    0,    0,    0,    0,    0,  218,  219,    0,  229,
    0,    0,    0,  220,    0,    0,   65,    0,    0,    0,
    0,  221,    0,    0,    0,   54,    0,    0,  127,    0,
   64,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,  227,   40,   41,  125,   42,   43,    0,   63,   45,
    0,    0,   46,   47,   48,   49,   50,    0,   51,    0,
    0,   65,    0,    0,  228,   35,   35,    0,    0,   35,
   35,   64,   35,    0,    0,    0,    0,   55,   56,   57,
   58,   59,   60,   61,   62,    0,    0,   35,   35,    0,
    0,    0,    0,   63,    0,  229,    0,    0,    0,    0,
    0,    0,   65,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,  222,   54,    0,   23,  108,    0,
    0,   64,    0,   35,    0,    0,   24,  227,    0,  223,
  224,  225,  226,    0,   63,   25,   26,   27,   28,   29,
    0,    0,   30,    0,    0,    0,    0,    0,    0,    0,
    0,    0,   65,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,   55,   56,   57,
    0,    0,   60,   61,   62,  211,  212,  213,    0,  214,
  215,  216,   64,  217,   63,    0,    0,    0,    0,   23,
  218,  219,    0,    0,    0,    0,    0,  220,   24,    0,
  435,    0,    0,    0,    0,  221,    0,   25,   26,   27,
   28,   29,  127,   65,   30,    0,  228,  512,    0,    0,
    0,    0,    0,    0,    0,    0,   40,   41,    0,   42,
   43,    0,    0,   45,  250,    0,   46,   47,   48,   49,
   50,    0,   51,   24,    0,   63,    0,  229,    0,    0,
    0,    0,   25,   26,   27,   28,   29,    0,  157,   30,
    0,  211,  212,  213,    0,  214,  215,  216,    0,  217,
    0,    0,    0,    0,  669,   23,  218,  219,    0,  227,
  535,    0,    0,  220,   24,    0,    0,    0,    0,  157,
    0,  221,    0,   25,   26,   27,   28,   29,  222,   54,
   30,    0,    0,    0,    0,  127,    0,    0,  158,    0,
    0,    0,    0,  223,  224,  225,  226,    0,   39,   40,
   41,  157,   42,   43,    0,   23,   45,    0,    0,   46,
   47,   48,   49,   50,   24,   51,    0,    0,    0,  158,
    0,    0,    0,   25,   26,   27,   28,   29,    0,    0,
   30,   55,   56,   57,    0,  127,   60,   61,   62,    0,
    0,    0,    0,    0,    0,    0,    0,    0,   39,   40,
   41,  158,   42,   43,    0,    0,   45,    0,    0,   46,
   47,   48,   49,   50,  222,   51,   23,    0,    0,    0,
    0,   64,   54,    0,    0,   24,    0,    0,    0,  223,
  224,  225,  226,    0,   25,   26,   27,   28,   29,    0,
    0,   30,    0,  211,  212,  213,    0,  214,  215,  216,
    0,  217,   65,    0,    0,    0,    0,    0,  218,  219,
    0,    0,  712,    0,    0,  220,    0,    0,    0,    0,
    0,    0,   54,  221,   55,   56,   57,   58,   59,   60,
   61,   62,    0,    0,   63,    0,    0,    0,    0,    0,
    0,    0,  157,    0,    0,    0,    0,    0,    0,    0,
    0,  157,    0,    0,    0,    0,    0,    0,    0,    0,
  157,  157,  157,  157,  157,    0,  519,  157,    0,    0,
    0,    0,  157,    0,   55,   56,   57,   58,   59,   60,
   61,   62,    0,    0,    0,  157,  157,  157,    0,  157,
  157,    0,  158,  157,    0,    0,  157,  157,  157,  157,
  157,  158,  157,    0,    0,    0,    0,  152,    0,    0,
  158,  158,  158,  158,  158,    0,  222,  158,    0,    0,
  108,    0,  158,    0,    0,    0,    0,    0,    0,    0,
    0,  223,  224,  225,  226,  158,  158,  158,  152,  158,
  158,    0,    0,  158,    0,    0,  158,  158,  158,  158,
  158,    0,  158,    0,    0,    0,    0,    0,    0,  157,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
  152,    0,    0,    0,    0,   23,    0,    0,    0,  228,
    0,    0,    0,    0,   24,    0,    0,    0,    0,    0,
  153,    0,    0,   25,   26,   27,   28,   29,    0,    0,
   30,  817,  818,    0,    0,  127,    0,   64,    0,  158,
  229,  157,  157,  157,  157,  157,  157,  157,  157,   40,
   41,  153,   42,   43,    0,    0,   45,    0,    0,   46,
   47,   48,   49,   50,   93,   51,    0,    0,   65,    0,
    0,    0,  227,   64,  853,    0,  855,    0,    0,    0,
    0,    0,    0,  153,    0,    0,    0,    0,    0,    0,
    0,  158,  158,  158,  158,  158,  158,  158,  158,    0,
   63,   64,    0,    0,   65,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,   54,    0,    0,    0,    0,    0,    0,    0,
    0,    0,   65,    0,    0,    0,   63,    0,    0,    0,
    0,  152,    0,  908,  909,    0,    0,  912,  913,    0,
  152,    0,    0,    0,    0,    0,    0,    0,    0,  152,
  152,  152,  152,  152,   63,    0,  152,    0,    0,    0,
    0,  152,    0,    0,   55,   56,   57,    0,  939,   60,
   61,   62,    0,    0,    0,  152,  152,    0,  152,  152,
    0,    0,  152,    0,    0,  152,  152,  152,  152,  152,
  152,  152,    0,    0,    0,    0,  211,  212,  213,    0,
  214,  215,  216,    0,  217,    0,    0,    0,    0,    0,
    0,  218,  219,    0,  153,   64,    0,    0,  220,    0,
    0,    0,    0,  153,  976,  977,  221,    0,  980,  981,
    0,   23,  153,  153,  153,  153,  153,    0,    0,  153,
   24,    0,    0,    0,  153,    0,   65,    0,  152,   25,
   26,   27,   28,   29,    0,    0,   30,    0,  153,  153,
  575,  153,  153,    0,    0,  153,    0,   23,  153,  153,
  153,  153,  153,  153,  153,    0,   24,  661,   63,    0,
    0,    0,    0,    0,    0,   25,   26,   27,   28,   29,
    0,    0,   30,    0,    0,   23, 1031,    0,    0,    0,
  152,  152,  152,    0,   24,  152,  152,  152,    0,    0,
    0,    0,    0,   25,   26,   27,   28,   29,    0,  222,
   30,    0,    0,    0,    0,  127,    0,    0,    0,    0,
    0,  153,    0,    0,  223,  224,  225,  226,    0,   40,
   41,    0,   42,   43,  192,    0,   45,    0,    0,   46,
   47,   48,   49,   50,    0,   51,    0,    0,    0,    0,
    0,    0,    0,   64,    0,    0,    0,  521,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,  153,  153,  153,    0,  719,  153,  153,
  153,    0,  358,    0,   65,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,  359,    0,
    0,  360,   54,    0,    0,    0,    0,    0,    0,   23,
    0,    0,    0,    0,    0,    0,   63,    0,   24,    0,
    0,    0,    0,  190,    0,    0,    0,   25,   26,   27,
   28,   29,    0,    0,   30,    0,    0,    0,    0,    0,
  191,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,   55,   56,   57,    0,    0,   60,
   61,   62,  361,  362,  363,    0,  364,  365,    0,    0,
    0,  366,    0,  367,    0,    0,  368,  369,  370,  371,
  372,  373,  374,  375,  376,  377,    0,  378,  379,    0,
  380,  381,  382,  383,  384,  385,  386,  387,  388,  389,
  390,  391,  392,    0,  393,  394,   64,    0,    0,  358,
  395,  396,  397,  398,  399,    0,  400,  401,  402,  403,
  404,  405,  406,    0,  407,  359,    0,    0,  360,    0,
    0,  289,    0,    0,    0,  408,    0,   65,    0,    0,
    0,    0,    0,    0,    0,    0,    0,  409,  410,  411,
  412,    0,    0,    0,  413,    0,    0,   23,    0,    0,
    0,    0,    0,    0,    0,    0,   24,    0,    0,   63,
    0,  190,    0,    0,    0,   25,   26,   27,   28,   29,
    0,    0,   30,    0,    0,    0,    0,    0,  191,  361,
  362,  363,    0,  364,  365,    0,    0,    0,  366,    0,
  367,    0,    0,  368,  369,  370,  371,  372,  373,  374,
  375,  376,  377,    0,  378,  379,  741,  380,  381,  382,
  383,  384,  385,  386,  387,  388,  389,  390,  391,  392,
    0,  393,  394,    0,    0,   64,    0,  395,  396,  397,
  398,  399,    0,  400,  401,  402,  403,  404,  405,  406,
    0,  407,    0,    0,    0,    0,  287,    0,    0,    0,
    0,    0,  408,  289,    0,    0,   65,    0,    0,    0,
    0,    0,    0,    0,  409,  410,  411,  412,    0,  289,
    0,  413,  289,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,   63,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
   23,    0,    0,    0,    0,    0,    0,    0,    0,   24,
    0,    0,    0,    0,    0,    0,    0,    0,   25,   26,
   27,   28,   29,    0,    0,   30,    0,    0,    0,    0,
    0,  180,    0,  289,  289,  289,    0,  289,  289,    0,
    0,    0,  289,    0,  289,    0,    0,  289,  289,  289,
  289,  289,  289,  289,  289,  289,  289,    0,  289,  289,
    0,  289,  289,  289,  289,  289,  289,  289,  289,  289,
  289,  289,  289,  289,    0,  289,  289,    0,  287,  290,
    0,  289,  289,  289,  289,  289,    0,  289,  289,  289,
  289,  289,  289,  289,  287,  289,    0,  287,    0,    0,
    0,    0,    0,    0,    0,    0,  289,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,  289,  289,
  289,  289,  211,  212,  213,  289,  214,  215,  216,   23,
  217,    0,    0,    0,    0,    0,    0,    0,   24,    0,
    0,  479,    0,    0,  220,    0,    0,   25,   26,   27,
   28,   29,  221,    0,   30,    0,    0,    0,  287,  287,
  287,    0,  287,  287,    0,    0,    0,  287,    0,  287,
    0,   64,  287,  287,  287,  287,  287,  287,  287,  287,
  287,  287,    0,  287,  287,    0,  287,  287,  287,  287,
  287,  287,  287,  287,  287,  287,  287,  287,  287,    0,
  287,  287,   65,    0,  288,    0,  287,  287,  287,  287,
  287,  290,  287,  287,  287,  287,  287,  287,  287,    0,
  287,    0,    0,    0,    0,    0,    0,  290,    0,    0,
  290,  287,    0,    0,   63,    0,    0,    0,    0,    0,
    0,    0,    0,  287,  287,  287,  287,    0,    0,    0,
  287,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,  290,  290,  290,    0,  290,  290,    0,    0,    0,
  290,    0,  290,    0,    0,  290,  290,  290,  290,  290,
  290,  290,  290,  290,  290,    0,  290,  290,    0,  290,
  290,  290,  290,  290,  290,  290,  290,  290,  290,  290,
  290,  290,    0,  290,  290,    0,  288,  181,    0,  290,
  290,  290,  290,  290,    0,  290,  290,  290,  290,  290,
  290,  290,  288,  290,    0,  288,   64,    0,    0,    0,
    0,    0,    0,    0,  290,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,   23,  290,  290,  290,  290,
    0,    0,    0,  290,   24,    0,    0,   65,    0,    0,
    0,    0,    0,   25,   26,   27,   28,   29,    0,    0,
   30,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,  288,  288,  288,   63,
  288,  288,    0,    0,    0,  288,    0,  288,    0,    0,
  288,  288,  288,  288,  288,  288,  288,  288,  288,  288,
    0,  288,  288, 1037,  288,  288,  288,  288,  288,  288,
  288,  288,  288,  288,  288,  288,  288,    0,  288,  288,
  498,  499,   64,    0,  288,  288,  288,  288,  288,  848,
  288,  288,  288,  288,  288,  288,  288,    0,  288,    0,
    0,    0,    0,    0,    0,    0,    0,    0,   64,  288,
    0,    0,    0,   65,    0,    0,    0,    0,    0,    0,
    0,  288,  288,  288,  288,    0,    0,    0,  288,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,   65,
    0,   64,    0,    0,    0,   63,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,   63,   65,    0,    0,   64,    0,    0,    0,    0,
   23,    0,    0,    0,    0,    0,    0,    0,    0,   24,
    0,    0,    0,    0,    0,    0,    0,    0,   25,   26,
   27,   28,   29,    0,   63,   30,   65,    0,   64,    0,
    0,  180,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,   64,    0,    0,    0,   63,   65,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,   64,    0,    0,
    0,    0,    0,    0,    0,   65,    0,    0,    0,    0,
    0,   63,    0,    0,    0,    0,    0,    0,    0,  211,
  212,  213,    0,  214,  215,  216,   23,  217,   65,    0,
    0,    0,    0,   64,    0,   24,    0,   63,  479,  253,
    0,  220,    0,    0,   25,   26,   27,   28,   29,  221,
    0,   30,   23,    0,    0,    0,    0,    0,    0,   64,
   63,   24,    0,    0,   65,    0,  847,   64,    0,    0,
   25,   26,   27,   28,   29,    0,    0,   30,  211,  212,
  213,  458,  214,  215,  216,   23,  459,    0,    0,    0,
   65,    0,    0,    0,   24,  460,   63,  461,   65,    0,
  220,    0,   64,   25,   26,   27,   28,   29,  221,    0,
   30,    0,  211,  212,  213,  458,  214,  215,  216,   23,
  459,    0,   63,    0,  115,    0,   64,    0,   24,  469,
   63,  461,  115,   65,  220,    0,    0,   25,   26,   27,
   28,   29,  221,    0,   30,  211,  212,  213,    0,  214,
  215,  216,   23,  217,   64,    0,    0,   65,    0,  992,
    0,   24,   64,    0,  479,   63,    0,  220,    0,    0,
   25,   26,   27,   28,   29,  221,    0,   30,  250,  251,
    0,    0,    0,    0,    0,   65,    0,   24,  252,   63,
    0,    0,    0,   65,    0,    0,   25,   26,   27,   28,
   29,  250,  251,   30,   64,    0,    0,    0,    0,    0,
   24,  252,   64,    0,    0,    0,    0,   63,    0,   25,
   26,   27,   28,   29,    0,   63,   30,    0,    0,    0,
    0,    0,    0,    0,    0,   65,    0,  250,  475,  352,
    0,    0,    0,   65,    0,    0,   24,  476,    0,    0,
    0,    0,    0,    0,    0,   25,   26,   27,   28,   29,
    0,    0,   30,   23,    0,    0,    0,   63,    0,    0,
  352,  250,   24,    0,    0,  319,    0,    0,    0,    0,
   24,   25,   26,   27,   28,   29,    0,    0,   30,   25,
   26,   27,   28,   29,    0,    0,   30,    0,    0,    0,
    0,    0,  352,    0,    0,    0,   23,    0,    0,    0,
    0,    0,    0,    0,    0,   24,  931,    0,    0,    0,
    0,    0,    0,    0,   25,   26,   27,   28,   29,    0,
   23,   30,    0,    0,    0,    0,    0,    0,    0,   24,
    0,    0,    0,    0,    0,    0,    0,    0,   25,   26,
   27,   28,   29,    0,    0,   30,    0,    0,   23,    0,
    0,    0,    0,    0,    0,    0,   23,   24,    0,    0,
    0,    0,  847,    0,    0,   24,   25,   26,   27,   28,
   29,    0,    0,   30,   25,   26,   27,   28,   29,    0,
  358,   30,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,  359,    0,  250,  360,
    0,    0,    0,    0,    0,    0,  250,   24,    0,    0,
    0,    0,    0,    0,    0,   24,   25,   26,   27,   28,
   29,    0,    0,   30,   25,   26,   27,   28,   29,    0,
    0,   30,    0,  352,    0,    0,    0,    0,    0,    0,
    0,    0,  352,    0,    0,    0,    0,    0,    0,    0,
    0,  352,  352,  352,  352,  352,    0,    0,  352,    0,
  361,  362,  363,    0,  364,  365,    0,    0,    0,  366,
    0,  367,    0,    0,  368,  369,  370,  371,  372,  373,
  374,  375,  376,  377,    0,  378,  379,    0,  380,  381,
  382,  383,  384,  385,  386,  387,  388,  389,  390,  391,
  392,    0,  393,  394,    0,    0,    0,    0,  395,  396,
  397,  398,  399,    0,  400,  401,  402,  403,  404,  405,
  406,  286,  407,    0,  414,  414,    0,    0,    0,    0,
    0,    0,    0,  408,    0,    0,    0,    0,    0,    0,
    0,    0,  127,    0,    0,  409,  410,  411,  412,    0,
    0,    0,  413,    0,    0,    0,   40,   41,    0,   42,
   43,    0,    0,   45,    0,    0,   46,   47,   48,   49,
   50,    0,   51,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,  414,  414,
  414,    0,  414,  414,    0,    0,    0,  414,    0,  414,
    0,    0,  414,  414,  414,  414,  414,  414,  414,  414,
  414,  414,    0,  414,  414,    0,  414,  414,  414,  414,
  414,  414,  414,  414,  414,  414,  414,  414,  414,   54,
  414,  414,  108,  383,  383,    0,  414,  414,  414,  414,
  414,    0,  414,  414,  414,  414,  414,  414,  414,    0,
  414,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,  414,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,  414,  414,  414,  414,    0,    0,    0,
  414,   55,   56,   57,    0,    0,   60,   61,   62,    0,
    0,    0,    0,    0,    0,    0,    0,  383,  383,  383,
    0,  383,  383,    0,    0,    0,  383,    0,  383,    0,
    0,  383,  383,  383,  383,  383,  383,  383,  383,  383,
  383,    0,  383,  383,    0,  383,  383,  383,  383,  383,
  383,  383,  383,  383,  383,  383,  383,  383,    0,  383,
  383,  363,  363,    0,    0,  383,  383,  383,  383,  383,
    0,  383,  383,  383,  383,  383,  383,  383,    0,  383,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
  383,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,  383,  383,  383,  383,    0,    0,    0,  383,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,  363,  363,  363,    0,  363,
  363,    0,    0,    0,  363,    0,  363,    0,    0,  363,
  363,  363,  363,  363,  363,  363,  363,  363,  363,    0,
  363,  363,    0,  363,  363,  363,  363,  363,  363,  363,
  363,  363,  363,  363,  363,  363,    0,  363,  363,  369,
  369,    0,    0,  363,  363,  363,  363,  363,    0,  363,
  363,  363,  363,  363,  363,  363,    0,  363,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,  363,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
  363,  363,  363,  363,    0,    0,    0,  363,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,  369,  369,  369,    0,  369,  369,    0,
    0,    0,  369,    0,  369,    0,    0,  369,  369,  369,
  369,  369,  369,  369,  369,  369,  369,    0,  369,  369,
    0,  369,  369,  369,  369,  369,  369,  369,  369,  369,
  369,  369,  369,  369,    0,  369,  369,  364,  364,    0,
    0,  369,  369,  369,  369,  369,    0,  369,  369,  369,
  369,  369,  369,  369,    0,  369,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,  369,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,  369,  369,
  369,  369,    0,    0,    0,  369,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,  364,  364,  364,    0,  364,  364,    0,    0,    0,
  364,    0,  364,    0,    0,  364,  364,  364,  364,  364,
  364,  364,  364,  364,  364,    0,  364,  364,    0,  364,
  364,  364,  364,  364,  364,  364,  364,  364,  364,  364,
  364,  364,    0,  364,  364,  370,  370,    0,    0,  364,
  364,  364,  364,  364,    0,  364,  364,  364,  364,  364,
  364,  364,    0,  364,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,  364,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,  364,  364,  364,  364,
    0,    0,    0,  364,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,  370,
  370,  370,    0,  370,  370,    0,    0,    0,  370,    0,
  370,    0,    0,  370,  370,  370,  370,  370,  370,  370,
  370,  370,  370,    0,  370,  370,    0,  370,  370,  370,
  370,  370,  370,  370,  370,  370,  370,  370,  370,  370,
    0,  370,  370,  365,  365,    0,    0,  370,  370,  370,
  370,  370,    0,  370,  370,  370,  370,  370,  370,  370,
    0,  370,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,  370,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,  370,  370,  370,  370,    0,    0,
    0,  370,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,  365,  365,  365,
    0,  365,  365,    0,    0,    0,  365,    0,  365,    0,
    0,  365,  365,  365,  365,  365,  365,  365,  365,  365,
  365,    0,  365,  365,    0,  365,  365,  365,  365,  365,
  365,  365,  365,  365,  365,  365,  365,  365,    0,  365,
  365,  376,  376,    0,    0,  365,  365,  365,  365,  365,
    0,  365,  365,  365,  365,  365,  365,  365,    0,  365,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
  365,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,  365,  365,  365,  365,    0,    0,    0,  365,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,  376,  376,  376,    0,  376,
  376,    0,    0,    0,  376,    0,  376,    0,    0,  376,
  376,  376,  376,  376,  376,  376,  376,  376,  376,    0,
  376,  376,    0,  376,  376,  376,  376,  376,  376,  376,
  376,  376,  376,  376,  376,  376,    0,  376,  376,  400,
  400,    0,    0,  376,  376,  376,  376,  376,    0,  376,
  376,  376,  376,  376,  376,  376,    0,  376,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,  376,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
  376,  376,  376,  376,    0,    0,    0,  376,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,  400,  400,  400,    0,  400,  400,    0,
    0,    0,  400,    0,  400,    0,    0,  400,  400,  400,
  400,  400,  400,  400,  400,  400,  400,    0,  400,  400,
    0,  400,  400,  400,  400,  400,  400,  400,  400,  400,
  400,  400,  400,  400,    0,  400,  400,  394,  394,    0,
    0,  400,  400,  400,  400,  400,    0,  400,  400,  400,
  400,  400,  400,  400,    0,  400,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,  400,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,  400,  400,
  400,  400,    0,    0,    0,  400,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,  394,  394,  394,    0,  394,  394,    0,    0,    0,
  394,    0,  394,    0,    0,  394,  394,  394,  394,  394,
  394,  394,  394,  394,  394,    0,  394,  394,    0,  394,
  394,  394,  394,  394,  394,  394,  394,  394,  394,  394,
  394,  394,    0,  394,  394,  371,  371,    0,    0,  394,
  394,  394,  394,  394,    0,  394,  394,  394,  394,  394,
  394,  394,    0,  394,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,  394,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,  394,  394,  394,  394,
    0,    0,    0,  394,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,  371,
  371,  371,    0,  371,  371,    0,    0,    0,  371,    0,
  371,    0,    0,  371,  371,  371,  371,  371,  371,  371,
  371,  371,  371,    0,  371,  371,    0,  371,  371,  371,
  371,  371,  371,  371,  371,  371,  371,  371,  371,  371,
    0,  371,  371,  366,  366,    0,    0,  371,  371,  371,
  371,  371,    0,  371,  371,  371,  371,  371,  371,  371,
    0,  371,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,  371,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,  371,  371,  371,  371,    0,    0,
    0,  371,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,  366,  366,  366,
    0,  366,  366,    0,    0,    0,  366,    0,  366,    0,
    0,  366,  366,  366,  366,  366,  366,  366,  366,  366,
  366,    0,  366,  366,    0,  366,  366,  366,  366,  366,
  366,  366,  366,  366,  366,  366,  366,  366,    0,  366,
  366,  367,  367,    0,    0,  366,  366,  366,  366,  366,
    0,  366,  366,  366,  366,  366,  366,  366,    0,  366,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
  366,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,  366,  366,  366,  366,    0,    0,    0,  366,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,  367,  367,  367,    0,  367,
  367,    0,    0,    0,  367,    0,  367,    0,    0,  367,
  367,  367,  367,  367,  367,  367,  367,  367,  367,    0,
  367,  367,    0,  367,  367,  367,  367,  367,  367,  367,
  367,  367,  367,  367,  367,  367,    0,  367,  367,  372,
  372,    0,    0,  367,  367,  367,  367,  367,    0,  367,
  367,  367,  367,  367,  367,  367,    0,  367,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,  367,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
  367,  367,  367,  367,    0,    0,    0,  367,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,  372,  372,  372,    0,  372,  372,    0,
    0,    0,  372,    0,  372,    0,    0,  372,  372,  372,
  372,  372,  372,  372,  372,  372,  372,    0,  372,  372,
    0,  372,  372,  372,  372,  372,  372,  372,  372,  372,
  372,  372,  372,  372,    0,  372,  372,  381,  381,    0,
    0,  372,  372,  372,  372,  372,    0,  372,  372,  372,
  372,  372,  372,  372,    0,  372,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,  372,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,  372,  372,
  372,  372,    0,    0,    0,  372,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,  381,  381,  381,    0,  381,  381,    0,    0,    0,
  381,    0,  381,    0,    0,  381,  381,  381,  381,  381,
  381,  381,  381,  381,  381,    0,  381,  381,    0,  381,
  381,  381,  381,  381,  381,  381,  381,  381,  381,  381,
  381,  381,    0,  381,  381,  374,  374,    0,    0,  381,
  381,  381,  381,  381,    0,  381,  381,  381,  381,  381,
  381,  381,    0,  381,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,  381,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,  381,  381,  381,  381,
    0,    0,    0,  381,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,  374,
  374,  374,    0,  374,  374,    0,    0,    0,  374,    0,
  374,    0,    0,  374,  374,  374,  374,  374,  374,  374,
  374,  374,  374,    0,  374,  374,    0,  374,  374,  374,
  374,  374,  374,  374,  374,  374,  374,  374,  374,  374,
    0,  374,  374,  377,  377,    0,    0,  374,  374,  374,
  374,  374,    0,  374,  374,  374,  374,  374,  374,  374,
    0,  374,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,  374,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,  374,  374,  374,  374,    0,    0,
    0,  374,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,  377,  377,  377,
    0,  377,  377,    0,    0,    0,  377,    0,  377,    0,
    0,  377,  377,  377,  377,  377,  377,  377,  377,  377,
  377,    0,  377,  377,    0,  377,  377,  377,  377,  377,
  377,  377,  377,  377,  377,  377,  377,  377,    0,  377,
  377,  397,  397,    0,    0,  377,  377,  377,  377,  377,
    0,  377,  377,  377,  377,  377,  377,  377,    0,  377,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
  377,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,  377,  377,  377,  377,    0,    0,    0,  377,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,  397,  397,  397,    0,  397,
  397,    0,    0,    0,  397,    0,  397,    0,    0,  397,
  397,  397,  397,  397,  397,  397,  397,  397,  397,    0,
  397,  397,    0,  397,  397,  397,  397,  397,  397,  397,
  397,  397,  397,  397,  397,  397,    0,  397,  397,  395,
  395,    0,    0,  397,  397,  397,  397,  397,    0,  397,
  397,  397,  397,  397,  397,  397,    0,  397,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,  397,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
  397,  397,  397,  397,    0,    0,    0,  397,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,  395,  395,  395,    0,  395,  395,    0,
    0,    0,  395,    0,  395,    0,    0,  395,  395,  395,
  395,  395,  395,  395,  395,  395,  395,    0,  395,  395,
    0,  395,  395,  395,  395,  395,  395,  395,  395,  395,
  395,  395,  395,  395,    0,  395,  395,  368,  368,    0,
    0,  395,  395,  395,  395,  395,    0,  395,  395,  395,
  395,  395,  395,  395,    0,  395,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,  395,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,  395,  395,
  395,  395,    0,    0,    0,  395,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,  368,  368,  368,    0,  368,  368,    0,    0,    0,
  368,    0,  368,    0,    0,  368,  368,  368,  368,  368,
  368,  368,  368,  368,  368,    0,  368,  368,    0,  368,
  368,  368,  368,  368,  368,  368,  368,  368,  368,  368,
  368,  368,    0,  368,  368,  373,  373,    0,    0,  368,
  368,  368,  368,  368,    0,  368,  368,  368,  368,  368,
  368,  368,    0,  368,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,  368,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,  368,  368,  368,  368,
    0,    0,    0,  368,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,  373,
  373,  373,    0,  373,  373,    0,    0,    0,  373,    0,
  373,    0,    0,  373,  373,  373,  373,  373,  373,  373,
  373,  373,  373,    0,  373,  373,    0,  373,  373,  373,
  373,  373,  373,  373,  373,  373,  373,  373,  373,  373,
    0,  373,  373,  375,  375,    0,    0,  373,  373,  373,
  373,  373,    0,  373,  373,  373,  373,  373,  373,  373,
    0,  373,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,  373,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,  373,  373,  373,  373,    0,    0,
    0,  373,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,  375,  375,  375,
    0,  375,  375,    0,    0,    0,  375,    0,  375,    0,
    0,  375,  375,  375,  375,  375,  375,  375,  375,  375,
  375,    0,  375,  375,    0,  375,  375,  375,  375,  375,
  375,  375,  375,  375,  375,  375,  375,  375,    0,  375,
  375,  378,  378,    0,    0,  375,  375,  375,  375,  375,
    0,  375,  375,  375,  375,  375,  375,  375,    0,  375,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
  375,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,  375,  375,  375,  375,    0,    0,    0,  375,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,  378,  378,  378,    0,  378,
  378,    0,    0,    0,  378,    0,  378,    0,    0,  378,
  378,  378,  378,  378,  378,  378,  378,  378,  378,    0,
  378,  378,    0,  378,  378,  378,  378,  378,  378,  378,
  378,  378,  378,  378,  378,  378,    0,  378,  378,  379,
  379,    0,    0,  378,  378,  378,  378,  378,    0,  378,
  378,  378,  378,  378,  378,  378,    0,  378,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,  378,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
  378,  378,  378,  378,    0,    0,    0,  378,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,  379,  379,  379,    0,  379,  379,    0,
    0,    0,  379,    0,  379,    0,    0,  379,  379,  379,
  379,  379,  379,  379,  379,  379,  379,    0,  379,  379,
    0,  379,  379,  379,  379,  379,  379,  379,  379,  379,
  379,  379,  379,  379,    0,  379,  379,  396,  396,    0,
    0,  379,  379,  379,  379,  379,    0,  379,  379,  379,
  379,  379,  379,  379,    0,  379,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,  379,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,  379,  379,
  379,  379,    0,    0,    0,  379,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,  396,  396,  396,    0,  396,  396,    0,    0,    0,
  396,    0,  396,    0,    0,  396,  396,  396,  396,  396,
  396,  396,  396,  396,  396,    0,  396,  396,    0,  396,
  396,  396,  396,  396,  396,  396,  396,  396,  396,  396,
  396,  396,    0,  396,  396,  380,  380,    0,    0,  396,
  396,  396,  396,  396,    0,  396,  396,  396,  396,  396,
  396,  396,    0,  396,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,  396,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,  396,  396,  396,  396,
    0,    0,    0,  396,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,  380,
  380,  380,    0,  380,  380,    0,    0,    0,  380,    0,
  380,    0,    0,  380,  380,  380,  380,  380,  380,  380,
  380,  380,  380,    0,  380,  380,    0,  380,  380,  380,
  380,  380,  380,  380,  380,  380,  380,  380,  380,  380,
  359,  380,  380,    0,    0,    0,    0,  380,  380,  380,
  380,  380,    0,  380,  380,  380,  380,  380,  380,  380,
    0,  380,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,  380,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,  380,  380,  380,  380,    0,    0,
    0,  380,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,  361,  362,  363,    0,  364,  365,
    0,    0,    0,  366,    0,  367,    0,    0,  368,  369,
  370,  371,  372,  373,  374,  375,  376,  377,    0,  378,
  379,    0,  380,  381,  382,  383,  384,  385,  386,  387,
  388,  389,  390,  391,  392,  293,  393,  394,    0,    0,
    0,    0,  395,  396,  397,  398,  399,    0,  400,  401,
  402,  403,  404,  405,  406,    0,  407,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,  408,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,  409,
  410,  411,  412,    0,    0,    0,  413,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,  293,
  293,  293,    0,  293,  293,    0,    0,    0,  293,    0,
  293,    0,    0,  293,  293,  293,  293,  293,  293,  293,
  293,  293,  293,    0,  293,  293,    0,  293,  293,  293,
  293,  293,  293,  293,  293,  293,  293,  293,  293,  293,
  294,  293,  293,    0,    0,    0,    0,  293,  293,  293,
  293,  293,    0,  293,  293,  293,  293,  293,  293,  293,
    0,  293,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,  293,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,  293,  293,  293,  293,    0,    0,
    0,  293,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,  294,  294,  294,    0,  294,  294,
    0,    0,    0,  294,    0,  294,    0,    0,  294,  294,
  294,  294,  294,  294,  294,  294,  294,  294,    0,  294,
  294,    0,  294,  294,  294,  294,  294,  294,  294,  294,
  294,  294,  294,  294,  294,  295,  294,  294,    0,    0,
    0,    0,  294,  294,  294,  294,  294,    0,  294,  294,
  294,  294,  294,  294,  294,    0,  294,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,  294,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,  294,
  294,  294,  294,    0,    0,    0,  294,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,  295,
  295,  295,    0,  295,  295,    0,    0,    0,  295,    0,
  295,    0,    0,  295,  295,  295,  295,  295,  295,  295,
  295,  295,  295,    0,  295,  295,    0,  295,  295,  295,
  295,  295,  295,  295,  295,  295,  295,  295,  295,  295,
  296,  295,  295,    0,    0,    0,    0,  295,  295,  295,
  295,  295,    0,  295,  295,  295,  295,  295,  295,  295,
    0,  295,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,  295,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,  295,  295,  295,  295,    0,    0,
    0,  295,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,  296,  296,  296,    0,  296,  296,
    0,    0,    0,  296,    0,  296,    0,    0,  296,  296,
  296,  296,  296,  296,  296,  296,  296,  296,    0,  296,
  296,    0,  296,  296,  296,  296,  296,  296,  296,  296,
  296,  296,  296,  296,  296,    0,  296,  296,    0,    0,
    0,    0,  296,  296,  296,  296,  296,    0,  296,  296,
  296,  296,  296,  296,  296,    0,  296,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,  296,    0,    0,
    0,    0,    0,    0,    0,    0,    0,  364,    0,  296,
  296,  296,  296,    0,  367,    0,  296,  368,  369,  370,
  371,  372,  373,  374,  375,  376,  377,    0,  378,  379,
    0,  380,  381,  382,  383,  384,  385,  386,  387,  388,
  389,  390,  391,  392,    0,  393,  394,    0,    0,    0,
    0,  395,  396,  397,  398,  399,    0,  400,  401,  402,
  403,  404,  405,  406,    0,  407,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,  408,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,  409,  410,
  411,  412,    0,    0,    0,  413,
  };
  protected static readonly short [] yyCheck = {             6,
  135,   44,   71,   16,  608,   44,   41,  199,   40,   16,
   79,    6,   41,   82,   44,   41,   41,   62,   41,  290,
   41,   40,  331,  332,  333,   41,   40,  623,   91,   93,
   33,   33,  414,  665,  138,   41,   60,  123,  123,   44,
   60,  350,   41,   40,  198,  123,   44,  123,   44,  123,
   44,   44,   44,   44,   67,   40,   63,  123,   40,   40,
   44,  123,  197,   70,   71,   44,  123,   91,   44,  846,
  316,   44,   79,   44,   44,   82,   83,  298,   44,   44,
   44,   40,  303,   40,  305,   60,  276,   61,   83,   96,
  307,  281,  280,  280,  270,  271,   20,  201,  290,  123,
  204,  263,  206,  123,  111,  112,  228,  229,  280,  301,
  123,  273,  280,  120,  276,  128,   91,  280,  125,   41,
  125,  128,   44,  130,  193,  348,  314,  134,   44,  352,
  137,  125,   61,  140,   71,  130,   61,  325,  384,  146,
   44,   41,  137,  150,   44,  140,  153,  154,  123,  156,
  746,  314,  929,  462,  786,  426,  788,  274,   95,  791,
   97,    0,  325,   41,   20,  300,   44,  174,  175,   61,
  177,   95,   44,   97,   30,   99,  100,   93,   40,  186,
   42,  292,  293,  257,  430,  296,  297,  307,  274,  274,
  486,  381,  488,  381,  381,  299,  274,  319,  274,  581,
  304,  325,  209,  210,   44,  292,  293,  428,  274,  381,
  431,  135,  274,  381,  485,  335,  336,  274,  381,  436,
  437,  125,  384,   44,  124,  266,  272,   41,  235,  236,
   44,   61,  239,   44,  241,  296,  297,  429,  325,   95,
  264,   97,  260,   99,  100,  286,   61,  103,   40,  273,
   42,   62,   44,  125,   41,  887,  852,   44,  282,  283,
  284,  285,  286,  270,  418,  289,  420,   41,  288,  274,
   44,  274,  274,  384,  277,  272,  384,  281,   41,  135,
  274,   44,  289,   40,  123,  125,  310,  272,   40,  264,
  272,  272,  287,   40,  369,  364,  371,  384,  273,  608,
   40,  610,  257,  378,  125,  312,  615,  282,  283,  284,
  285,  286,  319,  272,  289,  272,  436,  437,  166,   41,
  384,  384,   44,   41,  331,  332,   44,  257,   40,  274,
  349,  927,  307,  125,   41,  349,   40,   44,   42,  384,
   44,  274,  938,  350,  940,  384,   61,  203,  657,  384,
  382,  364,  260,   41,  384,  384,   44,  364,  384,  384,
  367,  384,  369,  384,  371,   40,  373,  374,  384,  376,
  377,  378,  379,  380,  381,  382,  383,   40,  384,  227,
  228,  229,  389,  390,  260,  384,  384,  394,  384,   40,
  994,  384,  384,  384,  317,  991,   40,   60,  411,  442,
  384,  249,  409,  123,  411,  384,  413,   40,  384,  276,
 1006,  384, 1008,  384,  384,  257,  411,  441,  384,  384,
  384,  125,   41,  577,   41,   44,  495,   44,   91,  268,
  269,  438,  267,  272,  273,  274,  275,   42,   41,  292,
  293,   44,  298,  296,  297,  599,  294,  303, 1044,  305,
 1046,  290,  291,   41,  267,  274,   44,  296,  297,  123,
  123,  436,  437,  257,  618,  313,  314,  315,  316, 1065,
  323,  319,   40,   41,   42,  123,   44,   40,  317,    0,
  370,   41,  372,  317,   44,  375,  325,  326,  495,  496,
   41,  123,  340,   44,  501,   41,  257,  504,   44,  506,
  507,  496,  509,  510,   41,  512,  381,  514,  325,   41,
   40,  518,  519,  361,  521,  363,   40,  365,  408,   40,
  410,   42,   40,   40,   44,  532,  533,   58,   41,   58,
   58,  384,  539,  540,  541,  542,  384,  385,  386,  387,
  388,  276,   61,  391,  257,   40,   62,  395,  396,  397,
  398,  399,  400,  401,  402,  403,  404,  405,  406,  566,
  573,  260,  569,  257,  571,  572,  573,  721,   44,  381,
   44,  381,  428,  882,  883,  431,  381,  572,  573,   44,
    0,  735,  736,  590,   44,  257,  593,  594,  595,  596,
  260,  260,   58,   61,  257,  258,  259,  445,  261,  262,
  263,  608,  265,  610,   58,  421,  334,  257,  615,  272,
  273,  314,  381,   44,   40,    6,  279,   40,   41,   42,
  627,   44,  424,  932,  287,   16,  268,  269,  390,  257,
  272,  273,  627,  275,  149,  501,  390,  390,  381,  390,
  506,  257,  381,  509,    0,  381,  715,   44,  290,  291,
   44,   40,  518,  570,  661,    0,  663,  124,  665,  273,
   44,   44,  669,   44,   44,  672,  673,  674,  675,  676,
  677,  678,  679,  680,  681,  682,  683,  831,   44,   70,
   71,   44,  817,  818,  326,  994,   44,  535,   79,  843,
  390,   82,   83,  208,  701,   40,  260,   42,  390,  390,
  390,  390,  257,  390,  711,  712,  390,  714,  715,  626,
  390,  628,  390,  390,  906,  907,  390,  380,  257,  390,
   62,  257,  237,  238,  257,  240,  381,  381,   44,   40,
  257,  257,  395,  396,  397,  398,   44,  128,  260,  130,
  255,  748,   44,   44,  592,   44,  137,  268,  269,  140,
   44,  272,  273,   44,  275,  266,   44,   44,   44,   60,
    0,   44,   44,   44,  427,   44,   44,  123,   44,  290,
  291,   44,  428,  908,  909,  260,  257,  912,  913,  786,
  849,  788,  974,  975,  791,   44,  978,  979,  257,   44,
   91,   91,  709,  710,  309,  186,  713,  390,  257,   44,
   40,   44,   42,   44,  257,  326,  654,  655,  656,   44,
  658,  659,   44,  328,   44,  330,  125,   44,   44,   44,
  827,   44,  123,  671,   44,   44,   44,   44,   44,   44,
   44,   44,  827,   44,  751, 1027, 1028, 1029, 1030,  846,
  847,  976,  977,   44,   44,  980,  981,   44,  268,  269,
   44,   44,  272,  273,   91,  275,  260,   44,   44,   40,
   44,   42,   44,   40, 1056,  390,   44,  381,   44,  717,
  290,  291,    0,   44,  381,  381,  381,   44,  726,   60,
  887,    0,  390,   44,   44,  381,   44,  894,  895,  257,
   44,  329,  390,  257,  381,  381, 1031,  317,  257,  381,
  257,   44,   44,   93,  821,  822,  326,   44,  825,  826,
   91,  260,  268,  269,  257,   40,  272,  273,  274,  275,
  382,  329,  929,  268,  269,   40,   40,  272,  273,  936,
  275,  446,  257,  329,  290,  291,   44,    0,  381,  934,
  296,  297,  123,  257,  257,  290,  291,  795,  334,  797,
  329,  260,  334,  381,  257,   44,  257,  258,  259,   44,
  261,  262,  263,   40,  265,  390,  390,  276,  390,  325,
  326,  272,  273,  364,  381,  381,  491,   40,  279,   42,
  257,  326,  257,  257,  832,  500,  287,  994,   44,   44,
  505,    0,  272,  508,  317,   10,  511,  104,  513,  916,
  515,  516,  517,    0,  272,  520,  272,  522,  523,  524,
  525,  320,  321,  322,  272,  272,  272,  248,  327,  328,
  411,  330,  331,  332,  333,  249,   20,  610,  268,  269,
  335,  284,  880,  881,  332,  275,    0,  120,  423,   67,
  289,  270,  988,   40,   60,   42,  882,  417,  417,  897,
  290,  291,  883, 1060,  487,  936,  904,  615,  814,  929,
  713,   -1,   -1,   -1, 1071,   -1, 1073, 1074, 1075,  487,
   -1,   -1,   -1,  921,   -1,   91,  257,  258,  259,  380,
  261,  262,  263,  384,  265,   -1,  326,   -1,   -1,   -1,
   -1,  272,  273,   -1,  395,  396,  397,  398,  279,   -1,
   -1,   -1,   -1,    0,  495,  496,  287,  123,  257,  258,
  259,   -1,  261,  262,  263,  630,  265,  632,   -1,  634,
  635,   -1,  637,  638,   -1,  640,   -1,  642,   -1,   -1,
  279,  646,  647,   -1,  649,   -1,   -1,   -1,  287,   -1,
  268,  269,   -1,   -1,  272,  273,   -1,  275,   -1,  268,
  269,  666,  667,  272,  273,   -1,  275,   -1,   -1,   -1,
   -1,   -1,  290,  291,   -1,   -1,   60,   -1,   -1,  684,
    0,  290,  291,   -1,   -1,   -1,   -1, 1025,   -1,   -1,
  571,  572,  573,   -1,    0,   -1,   -1,  702,   -1,  317,
  705,   -1, 1040, 1041, 1042,   -1,   -1,   91,  326,  380,
   -1,  125,   -1,   -1,   -1,  268,  269,  326,   -1,  272,
  273,   -1,  275,   -1,  395,  396,  397,  398,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,  290,  291,  123,
   -1,   -1,   -1, 1081,   -1,  384,  627,   -1,  753, 1087,
    0,  756,   -1,   -1,  759,   -1,  427,  762,  264,  764,
   -1,  766,  767,  768,   60,   -1,  771,  273,  773,  774,
  775,  776,   -1,  326,   -1,   -1,  282,  283,  284,  285,
  286,  268,  269,  289,  789,  272,  273,   -1,  275,   -1,
   40,   41,   42,   -1,   44,   91,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,  290,  291,   -1,   -1,  812,  899,   -1,
   60,   -1,   62,   -1,  268,  269,   -1,   -1,  272,  273,
   -1,  275,   -1,   -1,   -1,   -1,   -1,  123,   -1,  920,
  711,  712,   -1,  714,  715,   -1,  290,  291,   -1,  326,
   -1,   91,   -1,   93,  849,   -1,  260,   -1,   -1,   -1,
   -1,   -1,  857,  858,   -1,  860,  861,   -1,  863,  864,
   -1,  866,  276,  868,   -1,   -1,   -1,  872,  873,   -1,
  875,   -1,  326,  123,  965,  125,  967,  383,   -1,   -1,
  264,  268,  269,   -1,   -1,  272,  273,   -1,  275,  273,
   -1,   -1,   -1,  984,   -1,  900,   -1,  902,  282,  283,
  284,  285,  286,  290,  291,  289,  320,  321,  322,   -1,
  294,   -1,   -1,  327,  328,   -1,  330,  331,  332,  333,
   -1,   -1,   -1,  307,  308,  309,   -1,  311,  312,  934,
 1021,  315,   -1,   -1,  318,  319,  320,  321,  322,  326,
  324,   -1,   -1,   -1,   -1,   -1,  827,   -1,  268,  269,
   -1,   -1,  272,  273,    0,  275,   -1,   -1,  963,  964,
  125,   -1,  268,  269,   -1,  970,  272,  273,  264,  275,
  290,  291,   -1,   -1,   -1,   -1,   -1,  273,   -1,   -1,
   -1,   -1,   -1,   -1,  290,  291,  282,  283,  284,  285,
  286,   -1,   -1,  289,   40,   41,   42,  381,   44,   -1,
   -1,   -1, 1093, 1094,   -1,   -1,  326,  257,  258,  259,
   -1,  261,  262,  263,   60,  265,   62,   -1,  268,  269,
  326,   -1,  272,  273,  274,  275,  276,   -1,   -1,  279,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,  287,   -1,   -1,
  290,  291,   -1,   -1,  294,   91,   -1,   93,   -1,  433,
  434,  435,  436,  437,  438,  439,  440,   -1,  308,  309,
   -1,  311,  312,   -1,   -1,  315,  316,   -1,  318,  319,
  320,  321,  322,   -1,  324,   -1,  326,  123,   -1,  125,
   -1,   -1,   -1,   -1,   -1,  381,   -1,  337,  338,  339,
   -1,  341,  342,   -1,   -1,  260,  346,   -1,  348,   -1,
    0,  351,  352,  353,  354,  355,  356,  357,  358,  359,
  360,  276,  362,  363,   -1,  365,  366,  367,  368,  369,
  370,  371,  372,  373,  374,  375,  376,  377,   -1,  379,
  380,  381,   -1,   -1,   -1,  385,  386,  387,  388,  389,
   -1,  391,  392,  393,  394,  395,  396,  397,  398,  399,
   -1,   -1,   -1,   -1,   -1,  320,  321,  322,   -1,   -1,
  410,   -1,  327,  328,   -1,  330,  331,  332,  333,   -1,
   -1,   -1,  422,  423,  424,  425,   -1,  427,   -1,  429,
  430,   -1,   -1,  433,  434,  435,   -1,   -1,  438,  439,
  440,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   41,
   -1,   -1,   44,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,  257,  258,  259,   -1,  261,  262,  263,   60,  265,
   -1,   -1,  268,  269,   -1,   -1,  272,  273,  274,  275,
  276,   -1,    0,  279,   -1,   -1,   -1,   -1,   -1,   -1,
    0,  287,   -1,   -1,  290,  291,   -1,   -1,  294,   91,
   -1,   -1,   -1,   -1,  261,  262,   -1,   -1,   -1,   -1,
   -1,   -1,  308,  309,   -1,  311,  312,   -1,   -1,  315,
  316,   -1,  318,  319,  320,  321,  322,   -1,  324,   -1,
  326,  123,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,  337,  338,  339,   -1,  341,  342,   -1,   -1,    0,
  346,   -1,  348,   -1,   -1,  351,  352,  353,  354,  355,
  356,  357,  358,  359,  360,   -1,  362,  363,   -1,  365,
  366,  367,  368,  369,  370,  371,  372,  373,  374,  375,
  376,  377,   -1,  379,  380,   -1,   -1,   -1,  384,  385,
  386,  387,  388,  389,   -1,  391,  392,  393,  394,  395,
  396,  397,  398,  399,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,  123,  410,   -1,   -1,   -1,  268,  269,
   -1,   -1,  272,  273,   -1,  275,  422,  423,  424,  425,
   -1,  427,   -1,  429,  430,   -1,   -1,  433,  434,  435,
  290,  291,  438,  439,  440,  402,  403,  404,  405,   41,
   -1,   -1,   44,   -1,  411,  412,  413,  414,  415,  416,
  417,  418,  419,  420,   -1,  257,  258,  259,   60,  261,
  262,  263,  264,  265,    0,   -1,  326,   -1,   -1,   -1,
  272,  273,   -1,    0,   -1,   -1,   -1,  279,   -1,   -1,
  282,  283,  284,  285,  286,  287,   -1,  289,   -1,   91,
   -1,   -1,  294,    0,  400,  401,  402,  403,  404,  405,
  406,  407,  408,  409,   -1,   -1,  308,  309,   -1,  311,
  312,   -1,   -1,  315,   -1,   -1,  318,  319,  320,  321,
  322,  123,  324,   -1,   -1,   -1,   -1,   60,   -1,   -1,
   -1,   -1,   -1,   -1,    0,   -1,   -1,   -1,   -1,   -1,
  268,  269,   -1,   -1,  272,  273,   -1,  275,  268,  269,
   -1,   -1,  272,  273,  274,  275,   -1,   -1,   91,   -1,
   -1,   -1,  290,  291,   40,   41,   -1,   -1,   44,   -1,
  290,  291,   -1,   -1,   -1,   -1,  296,  297,  380,  381,
   -1,   -1,   -1,   -1,   -1,   -1,   62,   -1,   -1,   -1,
  123,   -1,   -1,  395,  396,  397,  398,  317,  326,   -1,
   -1,   -1,   -1,   -1,   -1,  325,  326,  268,  269,   -1,
   -1,  272,  273,   -1,  275,   -1,   -1,   93,  257,  258,
  259,   -1,  261,  262,  263,   -1,  265,   -1,   -1,  290,
  291,  433,  434,  435,   -1,   -1,  438,  439,  440,   -1,
  279,   -1,   -1,   -1,   -1,   -1,   -1,  123,  287,  125,
   -1,   -1,   -1,   -1,   -1,  257,  258,  259,   -1,  261,
  262,  263,  264,  265,   -1,  326,   -1,   -1,   -1,   -1,
  272,  273,   40,   -1,   42,   -1,   -1,  279,   -1,   -1,
  282,  283,  284,  285,  286,  287,   -1,  289,   -1,   -1,
   -1,   -1,  294,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
  261,  262,   -1,   -1,   -1,   -1,  308,  309,   -1,  311,
  312,   -1,   -1,  315,   -1,   -1,  318,  319,  320,  321,
  322,   -1,  324,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,  264,  268,  269,   -1,   -1,  272,  273,   60,  275,
  273,  268,  269,   -1,   -1,  272,  273,   -1,  275,  282,
  283,  284,  285,  286,  290,  291,  289,   -1,   -1,   -1,
   -1,  268,  269,  290,  291,  272,  273,   -1,  275,   91,
   -1,   -1,   -1,   -1,   -1,    0,   -1,   -1,  380,  381,
   40,  257,   42,  290,  291,   -1,   -1,   -1,   -1,   -1,
  326,   -1,   -1,  395,  396,  397,  398,  273,  274,  326,
  276,  123,  268,  269,   -1,   -1,  272,  273,   -1,  275,
   -1,   -1,   -1,   -1,   -1,   -1,  349,   -1,   -1,  326,
   -1,   -1,   -1,   -1,  290,  291,   -1,   -1,   -1,   -1,
   -1,  433,  434,  435,   -1,   -1,  438,  439,  440,   -1,
   -1,  402,  403,  404,  405,   -1,   -1,   -1,   -1,  382,
  411,  412,  413,  414,  415,  416,  417,  418,  419,  420,
  326,  337,  338,  339,   -1,  341,  342,   -1,   -1,   -1,
  346,   -1,  348,   -1,   -1,  351,  352,  353,  354,  355,
  356,  357,  358,  359,  360,    0,  362,  363,   -1,  365,
  366,  367,  368,  369,  370,  371,  372,  373,  374,  375,
  376,  377,   -1,  379,  380,  273,  274,   -1,   -1,  385,
  386,  387,  388,  389,  390,  391,  392,  393,  394,  395,
  396,  397,   40,  399,   42,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,  410,  257,  258,  259,   -1,  261,
  262,  263,   -1,  265,   -1,   -1,  422,  423,  424,  425,
  272,  273,   -1,  429,   -1,   -1,   -1,  279,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,  287,   -1,   -1,   -1,  337,
  338,  339,   -1,  341,  342,   -1,   -1,   -1,  346,   -1,
  348,   -1,   -1,  351,  352,  353,  354,  355,  356,  357,
  358,  359,  360,   -1,  362,  363,   -1,  365,  366,  367,
  368,  369,  370,  371,  372,  373,  374,  375,  376,  377,
   -1,  379,  380,  273,  274,   -1,   -1,  385,  386,  387,
  388,  389,   -1,  391,  392,  393,  394,  395,  396,  397,
   40,  399,   42,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,  410,  268,  269,   -1,   -1,  272,  273,   -1,
  275,   -1,   -1,   -1,  422,  423,  424,  425,  380,   -1,
   -1,  429,  384,   -1,   -1,  290,  291,   -1,   -1,   -1,
   -1,   -1,   -1,  395,  396,  397,  398,  337,  338,  339,
   -1,  341,  342,   -1,   -1,   -1,  346,   -1,  348,   -1,
   -1,  351,  352,  353,  354,  355,  356,  357,  358,  359,
  360,  326,  362,  363,   -1,  365,  366,  367,  368,  369,
  370,  371,  372,  373,  374,  375,  376,  377,   -1,  379,
  380,   -1,   -1,   -1,   -1,  385,  386,  387,  388,  389,
   -1,  391,  392,  393,  394,  395,  396,  397,   40,  399,
   42,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
  410,   -1,   -1,  268,  269,  273,  274,  272,  273,   -1,
  275,   -1,  422,  423,  424,  425,   -1,   -1,   -1,  429,
   -1,   -1,   -1,   -1,   -1,  290,  291,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,  257,  258,  259,   -1,
  261,  262,  263,   -1,  265,   -1,   -1,   -1,   -1,   -1,
  125,  272,  273,   -1,   -1,  276,   -1,   -1,  279,   -1,
   -1,  326,   -1,   -1,   -1,   -1,  287,   -1,   -1,  337,
  338,  339,   -1,  341,  342,   -1,   -1,   -1,  346,   -1,
  348,   -1,   -1,  351,  352,  353,  354,  355,  356,  357,
  358,  359,  360,    0,  362,  363,   -1,  365,  366,  367,
  368,  369,  370,  371,  372,  373,  374,  375,  376,  377,
   -1,  379,  380,  273,  274,   -1,   -1,  385,  386,  387,
  388,  389,   -1,  391,  392,  393,  394,  395,  396,  397,
   40,  399,   42,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,  410,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,  422,  423,  424,  425,   -1,  380,
   -1,  429,   -1,  384,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,  395,  396,  397,  337,  338,  339,
   -1,  341,  342,   -1,   -1,  260,  346,   -1,  348,   -1,
   -1,  351,  352,  353,  354,  355,  356,  357,  358,  359,
  360,  276,  362,  363,   -1,  365,  366,  367,  368,  369,
  370,  371,  372,  373,  374,  375,  376,  377,   -1,  379,
  380,  273,  274,   -1,   -1,  385,  386,  387,  388,  389,
   -1,  391,  392,  393,  394,  395,  396,  397,   40,  399,
   42,   -1,   -1,   -1,   -1,  320,  321,  322,   -1,   -1,
  410,   -1,  327,  328,   -1,  330,  331,  332,  333,   -1,
   -1,   -1,  422,  423,  424,  425,   -1,  292,  293,  429,
   -1,  296,  297,  298,  299,  300,  301,  302,  303,  304,
  305,  306,   -1,   -1,   -1,  337,  338,  339,  313,  341,
  342,   -1,   -1,   -1,  346,   -1,  348,   -1,  323,  351,
  352,  353,  354,  355,  356,  357,  358,  359,  360,   -1,
  362,  363,   -1,  365,  366,  367,  368,  369,  370,  371,
  372,  373,  374,  375,  376,  377,   -1,  379,  380,   -1,
   -1,   -1,   -1,  385,  386,  387,  388,  389,   -1,  391,
  392,  393,  394,  395,  396,  397,   40,  399,   42,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,  410,  384,
  273,  268,  269,  273,  274,  272,  273,   -1,  275,   -1,
  422,  423,  424,  425,   -1,   -1,   -1,  429,   -1,   -1,
   -1,  294,   -1,  290,  291,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,  308,  309,   -1,  311,  312,
   -1,   -1,  315,   -1,   -1,  318,  319,  320,  321,  322,
   -1,  324,   -1,   -1,   -1,   -1,   -1,   -1,   -1,  326,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,  337,  338,  339,
   -1,  341,  342,   -1,   -1,   -1,  346,   -1,  348,   -1,
   -1,  351,  352,  353,  354,  355,  356,  357,  358,  359,
  360,    0,  362,  363,   -1,  365,  366,  367,  368,  369,
  370,  371,  372,  373,  374,  375,  376,  377,  381,  379,
  380,  273,  274,   -1,   -1,  385,  386,  387,  388,  389,
   -1,  391,  392,  393,  394,  395,  396,  397,   40,  399,
   42,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
  410,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,  422,  423,  424,  425,   -1,   -1,   -1,  429,
  433,  434,  435,   -1,   -1,  438,  439,  440,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,  337,  338,  339,   -1,  341,
  342,   -1,   -1,   -1,  346,   -1,  348,   -1,   -1,  351,
  352,  353,  354,  355,  356,  357,  358,  359,  360,   -1,
  362,  363,   -1,  365,  366,  367,  368,  369,  370,  371,
  372,  373,  374,  375,  376,  377,   -1,  379,  380,  273,
  274,   -1,   -1,  385,  386,  387,  388,  389,   -1,  391,
  392,  393,  394,  395,  396,  397,   40,  399,   42,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,  410,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
  422,  423,  424,  425,   -1,   -1,   -1,  429,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,  337,  338,  339,   -1,  341,  342,   -1,
   -1,   -1,  346,   -1,  348,   -1,   -1,  351,  352,  353,
  354,  355,  356,  357,  358,  359,  360,   -1,  362,  363,
   -1,  365,  366,  367,  368,  369,  370,  371,  372,  373,
  374,  375,  376,  377,   -1,  379,  380,   -1,   -1,   -1,
   -1,  385,  386,  387,  388,  389,   -1,  391,  392,  393,
  394,  395,  396,  397,   40,  399,   42,   95,   -1,   97,
   98,   99,  100,   -1,   -1,   -1,  410,   -1,   -1,  268,
  269,  273,  274,  272,  273,   -1,  275,   -1,  422,  423,
  424,  425,   -1,   -1,   -1,  429,   -1,   -1,   -1,   -1,
   -1,  290,  291,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,  147,
  148,   -1,   -1,  151,  152,   -1,   -1,  155,   -1,  157,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,  326,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,  337,  338,  339,   -1,  341,
  342,   -1,   -1,   -1,  346,   -1,  348,   -1,   -1,  351,
  352,  353,  354,  355,  356,  357,  358,  359,  360,   -1,
  362,  363,   -1,  365,  366,  367,  368,  369,  370,  371,
  372,  373,  374,  375,  376,  377,   -1,  379,  380,  273,
  274,   -1,   -1,  385,  386,  387,  388,  389,   -1,  391,
  392,  393,  394,  395,  396,  397,   40,  399,   42,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,  410,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
  422,  423,  424,  425,   -1,   -1,   -1,  429,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,  337,  338,  339,   -1,  341,  342,   -1,
   -1,   -1,  346,   -1,  348,   -1,   -1,  351,  352,  353,
  354,  355,  356,  357,  358,  359,  360,   -1,  362,  363,
   -1,  365,  366,  367,  368,  369,  370,  371,  372,  373,
  374,  375,  376,  377,   -1,  379,  380,  273,  274,   -1,
   -1,  385,  386,  387,  388,  389,   -1,  391,  392,  393,
  394,  395,  396,  397,   -1,  399,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,  410,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,  422,  423,
  424,  425,   -1,   -1,   -1,  429,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,  337,  338,  339,   -1,  341,  342,   -1,   -1,   -1,
  346,   -1,  348,   -1,   60,  351,  352,  353,  354,  355,
  356,  357,  358,  359,  360,   -1,  362,  363,   -1,  365,
  366,  367,  368,  369,  370,  371,  372,  373,  374,  375,
  376,  377,   -1,  379,  380,   91,   -1,   -1,   -1,  385,
  386,  387,  388,  389,   -1,  391,  392,  393,  394,  395,
  396,  397,   -1,  399,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,  410,   -1,   -1,  123,   -1,  273,
  274,   -1,   -1,   -1,   -1,   -1,  422,  423,  424,  425,
   -1,   -1,   -1,  429,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   60,   -1,   -1,    0,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   91,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,  337,  338,  339,   -1,  341,  342,   -1,
   -1,   -1,  346,   -1,  348,   -1,   -1,  351,  352,  353,
  354,  355,  356,  357,  358,  359,  360,  123,  362,  363,
   -1,  365,  366,  367,  368,  369,  370,  371,  372,  373,
  374,  375,  376,  377,   -1,  379,  380,   -1,   -1,   -1,
   -1,  385,  386,  387,  388,  389,   -1,  391,  392,  393,
  394,  395,  396,  397,   -1,  399,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,  410,   -1,  264,   -1,
   -1,   -1,   -1,   60,   -1,   -1,   -1,  273,  422,  423,
  424,  425,   -1,   -1,   -1,  429,  282,  283,  284,  285,
  286,   -1,   -1,  289,   -1,   -1,   -1,   -1,  294,   -1,
   -1,   -1,   -1,  299,   91,  301,  302,  303,  304,  305,
  306,  307,  308,  309,   -1,  311,  312,  313,   -1,  315,
   -1,   -1,  318,  319,  320,  321,  322,   -1,  324,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,  123,   -1,   60,  335,
  336,   -1,   -1,   -1,   -1,   60,   -1,   -1,   -1,   -1,
   -1,  257,  258,  259,   -1,  261,  262,  263,   -1,  265,
   -1,   -1,   -1,   -1,   -1,   -1,  272,  273,   -1,   91,
   -1,   -1,   -1,  279,   -1,   -1,   91,   -1,   -1,   -1,
   -1,  287,   -1,   -1,   -1,  381,   -1,   -1,  294,   -1,
   60,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,  123,  308,  309,   40,  311,  312,   -1,  123,  315,
   -1,   -1,  318,  319,  320,  321,  322,   -1,  324,   -1,
   -1,   91,   -1,   -1,   60,  268,  269,   -1,   -1,  272,
  273,   60,  275,   -1,   -1,   -1,   -1,  433,  434,  435,
  436,  437,  438,  439,  440,   -1,   -1,  290,  291,   -1,
   -1,   -1,   -1,  123,   -1,   91,   -1,   -1,   -1,   -1,
   -1,   -1,   91,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,  380,  381,   -1,  264,  384,   -1,
   -1,   60,   -1,  326,   -1,   -1,  273,  123,   -1,  395,
  396,  397,  398,   -1,  123,  282,  283,  284,  285,  286,
   -1,   -1,  289,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   91,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,  433,  434,  435,
   -1,   -1,  438,  439,  440,  257,  258,  259,   -1,  261,
  262,  263,   60,  265,  123,   -1,   -1,   -1,   -1,  264,
  272,  273,   -1,   -1,   -1,   -1,   -1,  279,  273,   -1,
   44,   -1,   -1,   -1,   -1,  287,   -1,  282,  283,  284,
  285,  286,  294,   91,  289,   -1,   60,  364,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,  308,  309,   -1,  311,
  312,   -1,   -1,  315,  264,   -1,  318,  319,  320,  321,
  322,   -1,  324,  273,   -1,  123,   -1,   91,   -1,   -1,
   -1,   -1,  282,  283,  284,  285,  286,   -1,   60,  289,
   -1,  257,  258,  259,   -1,  261,  262,  263,   -1,  265,
   -1,   -1,   -1,   -1,  349,  264,  272,  273,   -1,  123,
  310,   -1,   -1,  279,  273,   -1,   -1,   -1,   -1,   91,
   -1,  287,   -1,  282,  283,  284,  285,  286,  380,  381,
  289,   -1,   -1,   -1,   -1,  294,   -1,   -1,   60,   -1,
   -1,   -1,   -1,  395,  396,  397,  398,   -1,  307,  308,
  309,  123,  311,  312,   -1,  264,  315,   -1,   -1,  318,
  319,  320,  321,  322,  273,  324,   -1,   -1,   -1,   91,
   -1,   -1,   -1,  282,  283,  284,  285,  286,   -1,   -1,
  289,  433,  434,  435,   -1,  294,  438,  439,  440,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,  307,  308,
  309,  123,  311,  312,   -1,   -1,  315,   -1,   -1,  318,
  319,  320,  321,  322,  380,  324,  264,   -1,   -1,   -1,
   -1,   60,  381,   -1,   -1,  273,   -1,   -1,   -1,  395,
  396,  397,  398,   -1,  282,  283,  284,  285,  286,   -1,
   -1,  289,   -1,  257,  258,  259,   -1,  261,  262,  263,
   -1,  265,   91,   -1,   -1,   -1,   -1,   -1,  272,  273,
   -1,   -1,  421,   -1,   -1,  279,   -1,   -1,   -1,   -1,
   -1,   -1,  381,  287,  433,  434,  435,  436,  437,  438,
  439,  440,   -1,   -1,  123,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,  264,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,  273,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
  282,  283,  284,  285,  286,   -1,  364,  289,   -1,   -1,
   -1,   -1,  294,   -1,  433,  434,  435,  436,  437,  438,
  439,  440,   -1,   -1,   -1,  307,  308,  309,   -1,  311,
  312,   -1,  264,  315,   -1,   -1,  318,  319,  320,  321,
  322,  273,  324,   -1,   -1,   -1,   -1,   60,   -1,   -1,
  282,  283,  284,  285,  286,   -1,  380,  289,   -1,   -1,
  384,   -1,  294,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,  395,  396,  397,  398,  307,  308,  309,   91,  311,
  312,   -1,   -1,  315,   -1,   -1,  318,  319,  320,  321,
  322,   -1,  324,   -1,   -1,   -1,   -1,   -1,   -1,  381,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
  123,   -1,   -1,   -1,   -1,  264,   -1,   -1,   -1,   60,
   -1,   -1,   -1,   -1,  273,   -1,   -1,   -1,   -1,   -1,
   60,   -1,   -1,  282,  283,  284,  285,  286,   -1,   -1,
  289,  707,  708,   -1,   -1,  294,   -1,   60,   -1,  381,
   91,  433,  434,  435,  436,  437,  438,  439,  440,  308,
  309,   91,  311,  312,   -1,   -1,  315,   -1,   -1,  318,
  319,  320,  321,  322,  323,  324,   -1,   -1,   91,   -1,
   -1,   -1,  123,   60,  750,   -1,  752,   -1,   -1,   -1,
   -1,   -1,   -1,  123,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,  433,  434,  435,  436,  437,  438,  439,  440,   -1,
  123,   60,   -1,   -1,   91,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,  381,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   91,   -1,   -1,   -1,  123,   -1,   -1,   -1,
   -1,  264,   -1,  819,  820,   -1,   -1,  823,  824,   -1,
  273,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,  282,
  283,  284,  285,  286,  123,   -1,  289,   -1,   -1,   -1,
   -1,  294,   -1,   -1,  433,  434,  435,   -1,  854,  438,
  439,  440,   -1,   -1,   -1,  308,  309,   -1,  311,  312,
   -1,   -1,  315,   -1,   -1,  318,  319,  320,  321,  322,
  323,  324,   -1,   -1,   -1,   -1,  257,  258,  259,   -1,
  261,  262,  263,   -1,  265,   -1,   -1,   -1,   -1,   -1,
   -1,  272,  273,   -1,  264,   60,   -1,   -1,  279,   -1,
   -1,   -1,   -1,  273,  910,  911,  287,   -1,  914,  915,
   -1,  264,  282,  283,  284,  285,  286,   -1,   -1,  289,
  273,   -1,   -1,   -1,  294,   -1,   91,   -1,  381,  282,
  283,  284,  285,  286,   -1,   -1,  289,   -1,  308,  309,
  125,  311,  312,   -1,   -1,  315,   -1,  264,  318,  319,
  320,  321,  322,  323,  324,   -1,  273,  310,  123,   -1,
   -1,   -1,   -1,   -1,   -1,  282,  283,  284,  285,  286,
   -1,   -1,  289,   -1,   -1,  264,  982,   -1,   -1,   -1,
  433,  434,  435,   -1,  273,  438,  439,  440,   -1,   -1,
   -1,   -1,   -1,  282,  283,  284,  285,  286,   -1,  380,
  289,   -1,   -1,   -1,   -1,  294,   -1,   -1,   -1,   -1,
   -1,  381,   -1,   -1,  395,  396,  397,  398,   -1,  308,
  309,   -1,  311,  312,   41,   -1,  315,   -1,   -1,  318,
  319,  320,  321,  322,   -1,  324,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   60,   -1,   -1,   -1,  364,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,  433,  434,  435,   -1,  125,  438,  439,
  440,   -1,  257,   -1,   91,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,  273,   -1,
   -1,  276,  381,   -1,   -1,   -1,   -1,   -1,   -1,  264,
   -1,   -1,   -1,   -1,   -1,   -1,  123,   -1,  273,   -1,
   -1,   -1,   -1,  278,   -1,   -1,   -1,  282,  283,  284,
  285,  286,   -1,   -1,  289,   -1,   -1,   -1,   -1,   -1,
  295,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,  433,  434,  435,   -1,   -1,  438,
  439,  440,  337,  338,  339,   -1,  341,  342,   -1,   -1,
   -1,  346,   -1,  348,   -1,   -1,  351,  352,  353,  354,
  355,  356,  357,  358,  359,  360,   -1,  362,  363,   -1,
  365,  366,  367,  368,  369,  370,  371,  372,  373,  374,
  375,  376,  377,   -1,  379,  380,   60,   -1,   -1,  257,
  385,  386,  387,  388,  389,   -1,  391,  392,  393,  394,
  395,  396,  397,   -1,  399,  273,   -1,   -1,  276,   -1,
   -1,  125,   -1,   -1,   -1,  410,   -1,   91,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,  422,  423,  424,
  425,   -1,   -1,   -1,  429,   -1,   -1,  264,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,  273,   -1,   -1,  123,
   -1,  278,   -1,   -1,   -1,  282,  283,  284,  285,  286,
   -1,   -1,  289,   -1,   -1,   -1,   -1,   -1,  295,  337,
  338,  339,   -1,  341,  342,   -1,   -1,   -1,  346,   -1,
  348,   -1,   -1,  351,  352,  353,  354,  355,  356,  357,
  358,  359,  360,   -1,  362,  363,   41,  365,  366,  367,
  368,  369,  370,  371,  372,  373,  374,  375,  376,  377,
   -1,  379,  380,   -1,   -1,   60,   -1,  385,  386,  387,
  388,  389,   -1,  391,  392,  393,  394,  395,  396,  397,
   -1,  399,   -1,   -1,   -1,   -1,  125,   -1,   -1,   -1,
   -1,   -1,  410,  257,   -1,   -1,   91,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,  422,  423,  424,  425,   -1,  273,
   -1,  429,  276,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,  123,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
  264,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,  273,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,  282,  283,
  284,  285,  286,   -1,   -1,  289,   -1,   -1,   -1,   -1,
   -1,  295,   -1,  337,  338,  339,   -1,  341,  342,   -1,
   -1,   -1,  346,   -1,  348,   -1,   -1,  351,  352,  353,
  354,  355,  356,  357,  358,  359,  360,   -1,  362,  363,
   -1,  365,  366,  367,  368,  369,  370,  371,  372,  373,
  374,  375,  376,  377,   -1,  379,  380,   -1,  257,  125,
   -1,  385,  386,  387,  388,  389,   -1,  391,  392,  393,
  394,  395,  396,  397,  273,  399,   -1,  276,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,  410,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,  422,  423,
  424,  425,  257,  258,  259,  429,  261,  262,  263,  264,
  265,   -1,   -1,   -1,   -1,   -1,   -1,   -1,  273,   -1,
   -1,  276,   -1,   -1,  279,   -1,   -1,  282,  283,  284,
  285,  286,  287,   -1,  289,   -1,   -1,   -1,  337,  338,
  339,   -1,  341,  342,   -1,   -1,   -1,  346,   -1,  348,
   -1,   60,  351,  352,  353,  354,  355,  356,  357,  358,
  359,  360,   -1,  362,  363,   -1,  365,  366,  367,  368,
  369,  370,  371,  372,  373,  374,  375,  376,  377,   -1,
  379,  380,   91,   -1,  125,   -1,  385,  386,  387,  388,
  389,  257,  391,  392,  393,  394,  395,  396,  397,   -1,
  399,   -1,   -1,   -1,   -1,   -1,   -1,  273,   -1,   -1,
  276,  410,   -1,   -1,  123,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,  422,  423,  424,  425,   -1,   -1,   -1,
  429,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,  337,  338,  339,   -1,  341,  342,   -1,   -1,   -1,
  346,   -1,  348,   -1,   -1,  351,  352,  353,  354,  355,
  356,  357,  358,  359,  360,   -1,  362,  363,   -1,  365,
  366,  367,  368,  369,  370,  371,  372,  373,  374,  375,
  376,  377,   -1,  379,  380,   -1,  257,   41,   -1,  385,
  386,  387,  388,  389,   -1,  391,  392,  393,  394,  395,
  396,  397,  273,  399,   -1,  276,   60,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,  410,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,  264,  422,  423,  424,  425,
   -1,   -1,   -1,  429,  273,   -1,   -1,   91,   -1,   -1,
   -1,   -1,   -1,  282,  283,  284,  285,  286,   -1,   -1,
  289,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,  337,  338,  339,  123,
  341,  342,   -1,   -1,   -1,  346,   -1,  348,   -1,   -1,
  351,  352,  353,  354,  355,  356,  357,  358,  359,  360,
   -1,  362,  363,   41,  365,  366,  367,  368,  369,  370,
  371,  372,  373,  374,  375,  376,  377,   -1,  379,  380,
  349,  350,   60,   -1,  385,  386,  387,  388,  389,   41,
  391,  392,  393,  394,  395,  396,  397,   -1,  399,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   60,  410,
   -1,   -1,   -1,   91,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,  422,  423,  424,  425,   -1,   -1,   -1,  429,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   91,
   -1,   60,   -1,   -1,   -1,  123,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,  123,   91,   -1,   -1,   60,   -1,   -1,   -1,   -1,
  264,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,  273,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,  282,  283,
  284,  285,  286,   -1,  123,  289,   91,   -1,   60,   -1,
   -1,  295,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   60,   -1,   -1,   -1,  123,   91,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   60,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   91,   -1,   -1,   -1,   -1,
   -1,  123,   -1,   -1,   -1,   -1,   -1,   -1,   -1,  257,
  258,  259,   -1,  261,  262,  263,  264,  265,   91,   -1,
   -1,   -1,   -1,   60,   -1,  273,   -1,  123,  276,  125,
   -1,  279,   -1,   -1,  282,  283,  284,  285,  286,  287,
   -1,  289,  264,   -1,   -1,   -1,   -1,   -1,   -1,   60,
  123,  273,   -1,   -1,   91,   -1,  278,   60,   -1,   -1,
  282,  283,  284,  285,  286,   -1,   -1,  289,  257,  258,
  259,  260,  261,  262,  263,  264,  265,   -1,   -1,   -1,
   91,   -1,   -1,   -1,  273,  274,  123,  276,   91,   -1,
  279,   -1,   60,  282,  283,  284,  285,  286,  287,   -1,
  289,   -1,  257,  258,  259,  260,  261,  262,  263,  264,
  265,   -1,  123,   -1,  125,   -1,   60,   -1,  273,  274,
  123,  276,  125,   91,  279,   -1,   -1,  282,  283,  284,
  285,  286,  287,   -1,  289,  257,  258,  259,   -1,  261,
  262,  263,  264,  265,   60,   -1,   -1,   91,   -1,   93,
   -1,  273,   60,   -1,  276,  123,   -1,  279,   -1,   -1,
  282,  283,  284,  285,  286,  287,   -1,  289,  264,  265,
   -1,   -1,   -1,   -1,   -1,   91,   -1,  273,  274,  123,
   -1,   -1,   -1,   91,   -1,   -1,  282,  283,  284,  285,
  286,  264,  265,  289,   60,   -1,   -1,   -1,   -1,   -1,
  273,  274,   60,   -1,   -1,   -1,   -1,  123,   -1,  282,
  283,  284,  285,  286,   -1,  123,  289,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   91,   -1,  264,  265,   60,
   -1,   -1,   -1,   91,   -1,   -1,  273,  274,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,  282,  283,  284,  285,  286,
   -1,   -1,  289,  264,   -1,   -1,   -1,  123,   -1,   -1,
   91,  264,  273,   -1,   -1,  123,   -1,   -1,   -1,   -1,
  273,  282,  283,  284,  285,  286,   -1,   -1,  289,  282,
  283,  284,  285,  286,   -1,   -1,  289,   -1,   -1,   -1,
   -1,   -1,  123,   -1,   -1,   -1,  264,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,  273,  274,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,  282,  283,  284,  285,  286,   -1,
  264,  289,   -1,   -1,   -1,   -1,   -1,   -1,   -1,  273,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,  282,  283,
  284,  285,  286,   -1,   -1,  289,   -1,   -1,  264,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,  264,  273,   -1,   -1,
   -1,   -1,  278,   -1,   -1,  273,  282,  283,  284,  285,
  286,   -1,   -1,  289,  282,  283,  284,  285,  286,   -1,
  257,  289,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,  273,   -1,  264,  276,
   -1,   -1,   -1,   -1,   -1,   -1,  264,  273,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,  273,  282,  283,  284,  285,
  286,   -1,   -1,  289,  282,  283,  284,  285,  286,   -1,
   -1,  289,   -1,  264,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,  273,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,  282,  283,  284,  285,  286,   -1,   -1,  289,   -1,
  337,  338,  339,   -1,  341,  342,   -1,   -1,   -1,  346,
   -1,  348,   -1,   -1,  351,  352,  353,  354,  355,  356,
  357,  358,  359,  360,   -1,  362,  363,   -1,  365,  366,
  367,  368,  369,  370,  371,  372,  373,  374,  375,  376,
  377,   -1,  379,  380,   -1,   -1,   -1,   -1,  385,  386,
  387,  388,  389,   -1,  391,  392,  393,  394,  395,  396,
  397,  273,  399,   -1,  273,  274,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,  410,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,  294,   -1,   -1,  422,  423,  424,  425,   -1,
   -1,   -1,  429,   -1,   -1,   -1,  308,  309,   -1,  311,
  312,   -1,   -1,  315,   -1,   -1,  318,  319,  320,  321,
  322,   -1,  324,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,  337,  338,
  339,   -1,  341,  342,   -1,   -1,   -1,  346,   -1,  348,
   -1,   -1,  351,  352,  353,  354,  355,  356,  357,  358,
  359,  360,   -1,  362,  363,   -1,  365,  366,  367,  368,
  369,  370,  371,  372,  373,  374,  375,  376,  377,  381,
  379,  380,  384,  273,  274,   -1,  385,  386,  387,  388,
  389,   -1,  391,  392,  393,  394,  395,  396,  397,   -1,
  399,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,  410,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,  422,  423,  424,  425,   -1,   -1,   -1,
  429,  433,  434,  435,   -1,   -1,  438,  439,  440,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,  337,  338,  339,
   -1,  341,  342,   -1,   -1,   -1,  346,   -1,  348,   -1,
   -1,  351,  352,  353,  354,  355,  356,  357,  358,  359,
  360,   -1,  362,  363,   -1,  365,  366,  367,  368,  369,
  370,  371,  372,  373,  374,  375,  376,  377,   -1,  379,
  380,  273,  274,   -1,   -1,  385,  386,  387,  388,  389,
   -1,  391,  392,  393,  394,  395,  396,  397,   -1,  399,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
  410,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,  422,  423,  424,  425,   -1,   -1,   -1,  429,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,  337,  338,  339,   -1,  341,
  342,   -1,   -1,   -1,  346,   -1,  348,   -1,   -1,  351,
  352,  353,  354,  355,  356,  357,  358,  359,  360,   -1,
  362,  363,   -1,  365,  366,  367,  368,  369,  370,  371,
  372,  373,  374,  375,  376,  377,   -1,  379,  380,  273,
  274,   -1,   -1,  385,  386,  387,  388,  389,   -1,  391,
  392,  393,  394,  395,  396,  397,   -1,  399,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,  410,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
  422,  423,  424,  425,   -1,   -1,   -1,  429,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,  337,  338,  339,   -1,  341,  342,   -1,
   -1,   -1,  346,   -1,  348,   -1,   -1,  351,  352,  353,
  354,  355,  356,  357,  358,  359,  360,   -1,  362,  363,
   -1,  365,  366,  367,  368,  369,  370,  371,  372,  373,
  374,  375,  376,  377,   -1,  379,  380,  273,  274,   -1,
   -1,  385,  386,  387,  388,  389,   -1,  391,  392,  393,
  394,  395,  396,  397,   -1,  399,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,  410,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,  422,  423,
  424,  425,   -1,   -1,   -1,  429,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,  337,  338,  339,   -1,  341,  342,   -1,   -1,   -1,
  346,   -1,  348,   -1,   -1,  351,  352,  353,  354,  355,
  356,  357,  358,  359,  360,   -1,  362,  363,   -1,  365,
  366,  367,  368,  369,  370,  371,  372,  373,  374,  375,
  376,  377,   -1,  379,  380,  273,  274,   -1,   -1,  385,
  386,  387,  388,  389,   -1,  391,  392,  393,  394,  395,
  396,  397,   -1,  399,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,  410,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,  422,  423,  424,  425,
   -1,   -1,   -1,  429,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,  337,
  338,  339,   -1,  341,  342,   -1,   -1,   -1,  346,   -1,
  348,   -1,   -1,  351,  352,  353,  354,  355,  356,  357,
  358,  359,  360,   -1,  362,  363,   -1,  365,  366,  367,
  368,  369,  370,  371,  372,  373,  374,  375,  376,  377,
   -1,  379,  380,  273,  274,   -1,   -1,  385,  386,  387,
  388,  389,   -1,  391,  392,  393,  394,  395,  396,  397,
   -1,  399,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,  410,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,  422,  423,  424,  425,   -1,   -1,
   -1,  429,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,  337,  338,  339,
   -1,  341,  342,   -1,   -1,   -1,  346,   -1,  348,   -1,
   -1,  351,  352,  353,  354,  355,  356,  357,  358,  359,
  360,   -1,  362,  363,   -1,  365,  366,  367,  368,  369,
  370,  371,  372,  373,  374,  375,  376,  377,   -1,  379,
  380,  273,  274,   -1,   -1,  385,  386,  387,  388,  389,
   -1,  391,  392,  393,  394,  395,  396,  397,   -1,  399,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
  410,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,  422,  423,  424,  425,   -1,   -1,   -1,  429,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,  337,  338,  339,   -1,  341,
  342,   -1,   -1,   -1,  346,   -1,  348,   -1,   -1,  351,
  352,  353,  354,  355,  356,  357,  358,  359,  360,   -1,
  362,  363,   -1,  365,  366,  367,  368,  369,  370,  371,
  372,  373,  374,  375,  376,  377,   -1,  379,  380,  273,
  274,   -1,   -1,  385,  386,  387,  388,  389,   -1,  391,
  392,  393,  394,  395,  396,  397,   -1,  399,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,  410,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
  422,  423,  424,  425,   -1,   -1,   -1,  429,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,  337,  338,  339,   -1,  341,  342,   -1,
   -1,   -1,  346,   -1,  348,   -1,   -1,  351,  352,  353,
  354,  355,  356,  357,  358,  359,  360,   -1,  362,  363,
   -1,  365,  366,  367,  368,  369,  370,  371,  372,  373,
  374,  375,  376,  377,   -1,  379,  380,  273,  274,   -1,
   -1,  385,  386,  387,  388,  389,   -1,  391,  392,  393,
  394,  395,  396,  397,   -1,  399,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,  410,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,  422,  423,
  424,  425,   -1,   -1,   -1,  429,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,  337,  338,  339,   -1,  341,  342,   -1,   -1,   -1,
  346,   -1,  348,   -1,   -1,  351,  352,  353,  354,  355,
  356,  357,  358,  359,  360,   -1,  362,  363,   -1,  365,
  366,  367,  368,  369,  370,  371,  372,  373,  374,  375,
  376,  377,   -1,  379,  380,  273,  274,   -1,   -1,  385,
  386,  387,  388,  389,   -1,  391,  392,  393,  394,  395,
  396,  397,   -1,  399,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,  410,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,  422,  423,  424,  425,
   -1,   -1,   -1,  429,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,  337,
  338,  339,   -1,  341,  342,   -1,   -1,   -1,  346,   -1,
  348,   -1,   -1,  351,  352,  353,  354,  355,  356,  357,
  358,  359,  360,   -1,  362,  363,   -1,  365,  366,  367,
  368,  369,  370,  371,  372,  373,  374,  375,  376,  377,
   -1,  379,  380,  273,  274,   -1,   -1,  385,  386,  387,
  388,  389,   -1,  391,  392,  393,  394,  395,  396,  397,
   -1,  399,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,  410,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,  422,  423,  424,  425,   -1,   -1,
   -1,  429,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,  337,  338,  339,
   -1,  341,  342,   -1,   -1,   -1,  346,   -1,  348,   -1,
   -1,  351,  352,  353,  354,  355,  356,  357,  358,  359,
  360,   -1,  362,  363,   -1,  365,  366,  367,  368,  369,
  370,  371,  372,  373,  374,  375,  376,  377,   -1,  379,
  380,  273,  274,   -1,   -1,  385,  386,  387,  388,  389,
   -1,  391,  392,  393,  394,  395,  396,  397,   -1,  399,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
  410,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,  422,  423,  424,  425,   -1,   -1,   -1,  429,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,  337,  338,  339,   -1,  341,
  342,   -1,   -1,   -1,  346,   -1,  348,   -1,   -1,  351,
  352,  353,  354,  355,  356,  357,  358,  359,  360,   -1,
  362,  363,   -1,  365,  366,  367,  368,  369,  370,  371,
  372,  373,  374,  375,  376,  377,   -1,  379,  380,  273,
  274,   -1,   -1,  385,  386,  387,  388,  389,   -1,  391,
  392,  393,  394,  395,  396,  397,   -1,  399,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,  410,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
  422,  423,  424,  425,   -1,   -1,   -1,  429,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,  337,  338,  339,   -1,  341,  342,   -1,
   -1,   -1,  346,   -1,  348,   -1,   -1,  351,  352,  353,
  354,  355,  356,  357,  358,  359,  360,   -1,  362,  363,
   -1,  365,  366,  367,  368,  369,  370,  371,  372,  373,
  374,  375,  376,  377,   -1,  379,  380,  273,  274,   -1,
   -1,  385,  386,  387,  388,  389,   -1,  391,  392,  393,
  394,  395,  396,  397,   -1,  399,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,  410,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,  422,  423,
  424,  425,   -1,   -1,   -1,  429,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,  337,  338,  339,   -1,  341,  342,   -1,   -1,   -1,
  346,   -1,  348,   -1,   -1,  351,  352,  353,  354,  355,
  356,  357,  358,  359,  360,   -1,  362,  363,   -1,  365,
  366,  367,  368,  369,  370,  371,  372,  373,  374,  375,
  376,  377,   -1,  379,  380,  273,  274,   -1,   -1,  385,
  386,  387,  388,  389,   -1,  391,  392,  393,  394,  395,
  396,  397,   -1,  399,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,  410,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,  422,  423,  424,  425,
   -1,   -1,   -1,  429,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,  337,
  338,  339,   -1,  341,  342,   -1,   -1,   -1,  346,   -1,
  348,   -1,   -1,  351,  352,  353,  354,  355,  356,  357,
  358,  359,  360,   -1,  362,  363,   -1,  365,  366,  367,
  368,  369,  370,  371,  372,  373,  374,  375,  376,  377,
   -1,  379,  380,  273,  274,   -1,   -1,  385,  386,  387,
  388,  389,   -1,  391,  392,  393,  394,  395,  396,  397,
   -1,  399,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,  410,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,  422,  423,  424,  425,   -1,   -1,
   -1,  429,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,  337,  338,  339,
   -1,  341,  342,   -1,   -1,   -1,  346,   -1,  348,   -1,
   -1,  351,  352,  353,  354,  355,  356,  357,  358,  359,
  360,   -1,  362,  363,   -1,  365,  366,  367,  368,  369,
  370,  371,  372,  373,  374,  375,  376,  377,   -1,  379,
  380,  273,  274,   -1,   -1,  385,  386,  387,  388,  389,
   -1,  391,  392,  393,  394,  395,  396,  397,   -1,  399,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
  410,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,  422,  423,  424,  425,   -1,   -1,   -1,  429,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,  337,  338,  339,   -1,  341,
  342,   -1,   -1,   -1,  346,   -1,  348,   -1,   -1,  351,
  352,  353,  354,  355,  356,  357,  358,  359,  360,   -1,
  362,  363,   -1,  365,  366,  367,  368,  369,  370,  371,
  372,  373,  374,  375,  376,  377,   -1,  379,  380,  273,
  274,   -1,   -1,  385,  386,  387,  388,  389,   -1,  391,
  392,  393,  394,  395,  396,  397,   -1,  399,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,  410,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
  422,  423,  424,  425,   -1,   -1,   -1,  429,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,  337,  338,  339,   -1,  341,  342,   -1,
   -1,   -1,  346,   -1,  348,   -1,   -1,  351,  352,  353,
  354,  355,  356,  357,  358,  359,  360,   -1,  362,  363,
   -1,  365,  366,  367,  368,  369,  370,  371,  372,  373,
  374,  375,  376,  377,   -1,  379,  380,  273,  274,   -1,
   -1,  385,  386,  387,  388,  389,   -1,  391,  392,  393,
  394,  395,  396,  397,   -1,  399,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,  410,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,  422,  423,
  424,  425,   -1,   -1,   -1,  429,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,  337,  338,  339,   -1,  341,  342,   -1,   -1,   -1,
  346,   -1,  348,   -1,   -1,  351,  352,  353,  354,  355,
  356,  357,  358,  359,  360,   -1,  362,  363,   -1,  365,
  366,  367,  368,  369,  370,  371,  372,  373,  374,  375,
  376,  377,   -1,  379,  380,  273,  274,   -1,   -1,  385,
  386,  387,  388,  389,   -1,  391,  392,  393,  394,  395,
  396,  397,   -1,  399,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,  410,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,  422,  423,  424,  425,
   -1,   -1,   -1,  429,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,  337,
  338,  339,   -1,  341,  342,   -1,   -1,   -1,  346,   -1,
  348,   -1,   -1,  351,  352,  353,  354,  355,  356,  357,
  358,  359,  360,   -1,  362,  363,   -1,  365,  366,  367,
  368,  369,  370,  371,  372,  373,  374,  375,  376,  377,
   -1,  379,  380,  273,  274,   -1,   -1,  385,  386,  387,
  388,  389,   -1,  391,  392,  393,  394,  395,  396,  397,
   -1,  399,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,  410,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,  422,  423,  424,  425,   -1,   -1,
   -1,  429,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,  337,  338,  339,
   -1,  341,  342,   -1,   -1,   -1,  346,   -1,  348,   -1,
   -1,  351,  352,  353,  354,  355,  356,  357,  358,  359,
  360,   -1,  362,  363,   -1,  365,  366,  367,  368,  369,
  370,  371,  372,  373,  374,  375,  376,  377,   -1,  379,
  380,  273,  274,   -1,   -1,  385,  386,  387,  388,  389,
   -1,  391,  392,  393,  394,  395,  396,  397,   -1,  399,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
  410,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,  422,  423,  424,  425,   -1,   -1,   -1,  429,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,  337,  338,  339,   -1,  341,
  342,   -1,   -1,   -1,  346,   -1,  348,   -1,   -1,  351,
  352,  353,  354,  355,  356,  357,  358,  359,  360,   -1,
  362,  363,   -1,  365,  366,  367,  368,  369,  370,  371,
  372,  373,  374,  375,  376,  377,   -1,  379,  380,  273,
  274,   -1,   -1,  385,  386,  387,  388,  389,   -1,  391,
  392,  393,  394,  395,  396,  397,   -1,  399,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,  410,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
  422,  423,  424,  425,   -1,   -1,   -1,  429,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,  337,  338,  339,   -1,  341,  342,   -1,
   -1,   -1,  346,   -1,  348,   -1,   -1,  351,  352,  353,
  354,  355,  356,  357,  358,  359,  360,   -1,  362,  363,
   -1,  365,  366,  367,  368,  369,  370,  371,  372,  373,
  374,  375,  376,  377,   -1,  379,  380,  273,  274,   -1,
   -1,  385,  386,  387,  388,  389,   -1,  391,  392,  393,
  394,  395,  396,  397,   -1,  399,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,  410,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,  422,  423,
  424,  425,   -1,   -1,   -1,  429,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,  337,  338,  339,   -1,  341,  342,   -1,   -1,   -1,
  346,   -1,  348,   -1,   -1,  351,  352,  353,  354,  355,
  356,  357,  358,  359,  360,   -1,  362,  363,   -1,  365,
  366,  367,  368,  369,  370,  371,  372,  373,  374,  375,
  376,  377,   -1,  379,  380,  273,  274,   -1,   -1,  385,
  386,  387,  388,  389,   -1,  391,  392,  393,  394,  395,
  396,  397,   -1,  399,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,  410,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,  422,  423,  424,  425,
   -1,   -1,   -1,  429,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,  337,
  338,  339,   -1,  341,  342,   -1,   -1,   -1,  346,   -1,
  348,   -1,   -1,  351,  352,  353,  354,  355,  356,  357,
  358,  359,  360,   -1,  362,  363,   -1,  365,  366,  367,
  368,  369,  370,  371,  372,  373,  374,  375,  376,  377,
  273,  379,  380,   -1,   -1,   -1,   -1,  385,  386,  387,
  388,  389,   -1,  391,  392,  393,  394,  395,  396,  397,
   -1,  399,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,  410,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,  422,  423,  424,  425,   -1,   -1,
   -1,  429,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,  337,  338,  339,   -1,  341,  342,
   -1,   -1,   -1,  346,   -1,  348,   -1,   -1,  351,  352,
  353,  354,  355,  356,  357,  358,  359,  360,   -1,  362,
  363,   -1,  365,  366,  367,  368,  369,  370,  371,  372,
  373,  374,  375,  376,  377,  273,  379,  380,   -1,   -1,
   -1,   -1,  385,  386,  387,  388,  389,   -1,  391,  392,
  393,  394,  395,  396,  397,   -1,  399,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,  410,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,  422,
  423,  424,  425,   -1,   -1,   -1,  429,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,  337,
  338,  339,   -1,  341,  342,   -1,   -1,   -1,  346,   -1,
  348,   -1,   -1,  351,  352,  353,  354,  355,  356,  357,
  358,  359,  360,   -1,  362,  363,   -1,  365,  366,  367,
  368,  369,  370,  371,  372,  373,  374,  375,  376,  377,
  273,  379,  380,   -1,   -1,   -1,   -1,  385,  386,  387,
  388,  389,   -1,  391,  392,  393,  394,  395,  396,  397,
   -1,  399,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,  410,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,  422,  423,  424,  425,   -1,   -1,
   -1,  429,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,  337,  338,  339,   -1,  341,  342,
   -1,   -1,   -1,  346,   -1,  348,   -1,   -1,  351,  352,
  353,  354,  355,  356,  357,  358,  359,  360,   -1,  362,
  363,   -1,  365,  366,  367,  368,  369,  370,  371,  372,
  373,  374,  375,  376,  377,  273,  379,  380,   -1,   -1,
   -1,   -1,  385,  386,  387,  388,  389,   -1,  391,  392,
  393,  394,  395,  396,  397,   -1,  399,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,  410,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,  422,
  423,  424,  425,   -1,   -1,   -1,  429,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,  337,
  338,  339,   -1,  341,  342,   -1,   -1,   -1,  346,   -1,
  348,   -1,   -1,  351,  352,  353,  354,  355,  356,  357,
  358,  359,  360,   -1,  362,  363,   -1,  365,  366,  367,
  368,  369,  370,  371,  372,  373,  374,  375,  376,  377,
  273,  379,  380,   -1,   -1,   -1,   -1,  385,  386,  387,
  388,  389,   -1,  391,  392,  393,  394,  395,  396,  397,
   -1,  399,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,  410,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,  422,  423,  424,  425,   -1,   -1,
   -1,  429,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,  337,  338,  339,   -1,  341,  342,
   -1,   -1,   -1,  346,   -1,  348,   -1,   -1,  351,  352,
  353,  354,  355,  356,  357,  358,  359,  360,   -1,  362,
  363,   -1,  365,  366,  367,  368,  369,  370,  371,  372,
  373,  374,  375,  376,  377,   -1,  379,  380,   -1,   -1,
   -1,   -1,  385,  386,  387,  388,  389,   -1,  391,  392,
  393,  394,  395,  396,  397,   -1,  399,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,  410,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,  341,   -1,  422,
  423,  424,  425,   -1,  348,   -1,  429,  351,  352,  353,
  354,  355,  356,  357,  358,  359,  360,   -1,  362,  363,
   -1,  365,  366,  367,  368,  369,  370,  371,  372,  373,
  374,  375,  376,  377,   -1,  379,  380,   -1,   -1,   -1,
   -1,  385,  386,  387,  388,  389,   -1,  391,  392,  393,
  394,  395,  396,  397,   -1,  399,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,  410,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,  422,  423,
  424,  425,   -1,   -1,   -1,  429,
  };

#line 1539 "Iril/IR/IR.jay"

}

#line default
namespace yydebug {
        using System;
	 internal interface yyDebug {
		 void push (int state, Object value);
		 void lex (int state, int token, string name, Object value);
		 void shift (int from, int to, int errorFlag);
		 void pop (int state);
		 void discard (int state, int token, string name, Object value);
		 void reduce (int from, int to, int rule, string text, int len);
		 void shift (int from, int to);
		 void accept (Object value);
		 void error (string message);
		 void reject ();
	 }
	 
	 class yyDebugSimple : yyDebug {
		 void println (string s){
			 System.Diagnostics.Debug.WriteLine (s);
		 }
		 
		 public void push (int state, Object value) {
			 println ("push\tstate "+state+"\tvalue "+value);
		 }
		 
		 public void lex (int state, int token, string name, Object value) {
			 println("lex\tstate "+state+"\treading "+name+"\tvalue "+value);
		 }
		 
		 public void shift (int from, int to, int errorFlag) {
			 switch (errorFlag) {
			 default:				// normally
				 println("shift\tfrom state "+from+" to "+to);
				 break;
			 case 0: case 1: case 2:		// in error recovery
				 println("shift\tfrom state "+from+" to "+to
					     +"\t"+errorFlag+" left to recover");
				 break;
			 case 3:				// normally
				 println("shift\tfrom state "+from+" to "+to+"\ton error");
				 break;
			 }
		 }
		 
		 public void pop (int state) {
			 println("pop\tstate "+state+"\ton error");
		 }
		 
		 public void discard (int state, int token, string name, Object value) {
			 println("discard\tstate "+state+"\ttoken "+name+"\tvalue "+value);
		 }
		 
		 public void reduce (int from, int to, int rule, string text, int len) {
			 println("reduce\tstate "+from+"\tuncover "+to
				     +"\trule ("+rule+") "+text);
		 }
		 
		 public void shift (int from, int to) {
			 println("goto\tfrom state "+from+" to "+to);
		 }
		 
		 public void accept (Object value) {
			 println("accept\tvalue "+value);
		 }
		 
		 public void error (string message) {
			 println("error\t"+message);
		 }
		 
		 public void reject () {
			 println("reject");
		 }
		 
	 }
}
// %token constants
 class Token {
  public const int INTEGER = 257;
  public const int HEX_INTEGER = 258;
  public const int FLOAT_LITERAL = 259;
  public const int STRING = 260;
  public const int TRUE = 261;
  public const int FALSE = 262;
  public const int UNDEF = 263;
  public const int VOID = 264;
  public const int NULL = 265;
  public const int LABEL = 266;
  public const int X = 267;
  public const int SOURCE_FILENAME = 268;
  public const int TARGET = 269;
  public const int DATALAYOUT = 270;
  public const int TRIPLE = 271;
  public const int GLOBAL_SYMBOL = 272;
  public const int LOCAL_SYMBOL = 273;
  public const int META_SYMBOL = 274;
  public const int META_SYMBOL_DEF = 275;
  public const int SYMBOL = 276;
  public const int DISTINCT = 277;
  public const int METADATA = 278;
  public const int CONSTANT_BYTES = 279;
  public const int SECTION = 280;
  public const int TYPE = 281;
  public const int HALF = 282;
  public const int FLOAT = 283;
  public const int DOUBLE = 284;
  public const int X86_FP80 = 285;
  public const int INTEGER_TYPE = 286;
  public const int ZEROINITIALIZER = 287;
  public const int OPAQUE = 288;
  public const int PTR = 289;
  public const int DEFINE = 290;
  public const int DECLARE = 291;
  public const int UNNAMED_ADDR = 292;
  public const int LOCAL_UNNAMED_ADDR = 293;
  public const int NOALIAS = 294;
  public const int ELLIPSIS = 295;
  public const int GLOBAL = 296;
  public const int CONSTANT = 297;
  public const int PRIVATE = 298;
  public const int INTERNAL = 299;
  public const int EXTERNAL = 300;
  public const int LINKONCE = 301;
  public const int LINKONCE_ODR = 302;
  public const int WEAK = 303;
  public const int WEAK_ODR = 304;
  public const int APPENDING = 305;
  public const int COMMON = 306;
  public const int FASTCC = 307;
  public const int SIGNEXT = 308;
  public const int ZEROEXT = 309;
  public const int VOLATILE = 310;
  public const int RETURNED = 311;
  public const int DEREFERENCEABLE = 312;
  public const int AVAILABLE_EXTERNALLY = 313;
  public const int PERSONALITY = 314;
  public const int SRET = 315;
  public const int CLEANUP = 316;
  public const int EXTERNALLY_INITIALIZED = 317;
  public const int NONNULL = 318;
  public const int NOCAPTURE = 319;
  public const int WRITEONLY = 320;
  public const int READONLY = 321;
  public const int READNONE = 322;
  public const int HIDDEN = 323;
  public const int BYVAL = 324;
  public const int ATTRIBUTE_GROUP_REF = 325;
  public const int ATTRIBUTES = 326;
  public const int NORECURSE = 327;
  public const int NOUNWIND = 328;
  public const int UNWIND = 329;
  public const int SPECULATABLE = 330;
  public const int SSP = 331;
  public const int UWTABLE = 332;
  public const int ARGMEMONLY = 333;
  public const int SEQ_CST = 334;
  public const int DSO_LOCAL = 335;
  public const int DSO_PREEMPTABLE = 336;
  public const int RET = 337;
  public const int BR = 338;
  public const int SWITCH = 339;
  public const int INDIRECTBR = 340;
  public const int INVOKE = 341;
  public const int RESUME = 342;
  public const int CATCHSWITCH = 343;
  public const int CATCHRET = 344;
  public const int CLEANUPRET = 345;
  public const int UNREACHABLE = 346;
  public const int FNEG = 347;
  public const int ADD = 348;
  public const int NUW = 349;
  public const int NSW = 350;
  public const int FADD = 351;
  public const int SUB = 352;
  public const int FSUB = 353;
  public const int MUL = 354;
  public const int FMUL = 355;
  public const int UDIV = 356;
  public const int SDIV = 357;
  public const int FDIV = 358;
  public const int UREM = 359;
  public const int SREM = 360;
  public const int FREM = 361;
  public const int SHL = 362;
  public const int LSHR = 363;
  public const int EXACT = 364;
  public const int ASHR = 365;
  public const int AND = 366;
  public const int OR = 367;
  public const int XOR = 368;
  public const int EXTRACTELEMENT = 369;
  public const int INSERTELEMENT = 370;
  public const int SHUFFLEVECTOR = 371;
  public const int EXTRACTVALUE = 372;
  public const int INSERTVALUE = 373;
  public const int ALLOCA = 374;
  public const int LOAD = 375;
  public const int STORE = 376;
  public const int FENCE = 377;
  public const int CMPXCHG = 378;
  public const int ATOMICRMW = 379;
  public const int GETELEMENTPTR = 380;
  public const int ALIGN = 381;
  public const int INBOUNDS = 382;
  public const int INRANGE = 383;
  public const int ADDRSPACE = 384;
  public const int TRUNC = 385;
  public const int ZEXT = 386;
  public const int SEXT = 387;
  public const int FPTRUNC = 388;
  public const int FPEXT = 389;
  public const int TO = 390;
  public const int FPTOUI = 391;
  public const int FPTOSI = 392;
  public const int UITOFP = 393;
  public const int SITOFP = 394;
  public const int PTRTOINT = 395;
  public const int INTTOPTR = 396;
  public const int BITCAST = 397;
  public const int ADDRSPACECAST = 398;
  public const int ICMP = 399;
  public const int EQ = 400;
  public const int NE = 401;
  public const int UGT = 402;
  public const int UGE = 403;
  public const int ULT = 404;
  public const int ULE = 405;
  public const int SGT = 406;
  public const int SGE = 407;
  public const int SLT = 408;
  public const int SLE = 409;
  public const int FCMP = 410;
  public const int OEQ = 411;
  public const int OGT = 412;
  public const int OGE = 413;
  public const int OLT = 414;
  public const int OLE = 415;
  public const int ONE = 416;
  public const int ORD = 417;
  public const int UEQ = 418;
  public const int UNE = 419;
  public const int UNO = 420;
  public const int FAST = 421;
  public const int PHI = 422;
  public const int SELECT = 423;
  public const int CALL = 424;
  public const int TAIL = 425;
  public const int VA_ARG = 426;
  public const int ASM = 427;
  public const int SIDEEFFECT = 428;
  public const int LANDINGPAD = 429;
  public const int CATCH = 430;
  public const int CATCHPAD = 431;
  public const int CLEANUPPAD = 432;
  public const int NOUNDEF = 433;
  public const int IMMARG = 434;
  public const int CAPTURES = 435;
  public const int SWIFTCC = 436;
  public const int SWIFTTAILCC = 437;
  public const int SWIFTSELF = 438;
  public const int SWIFTERROR = 439;
  public const int SWIFTASYNC = 440;
  public const int ATOMIC = 441;
  public const int MONOTONIC = 442;
  public const int yyErrorCode = 256;
 }
 namespace yyParser {
  using System;
  /** thrown for irrecoverable syntax errors and stack overflow.
    */
  internal class yyException : System.Exception {
    public yyException (string message) : base (message) {
    }
  }
  internal class yyUnexpectedEof : yyException {
    public yyUnexpectedEof (string message) : base (message) {
    }
    public yyUnexpectedEof () : base ("") {
    }
  }

  /** must be implemented by a scanner object to supply input to the parser.
    */
  internal interface yyInput {
    /** move on to next token.
        @return false if positioned beyond tokens.
        @throws IOException on input error.
      */
    bool advance (); // throws java.io.IOException;
    /** classifies current token.
        Should not be called if advance() returned false.
        @return current %token or single character.
      */
    int token ();
    /** associated with current token.
        Should not be called if advance() returned false.
        @return value for token().
      */
    Object value ();
  }
 }
} // close outermost namespace, that MUST HAVE BEEN opened in the prolog
